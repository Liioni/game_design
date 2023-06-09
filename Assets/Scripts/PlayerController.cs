using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float dashDistance;
    [SerializeField]
    private HealthBar health_bar;
    [SerializeField]
    private CooldownScript teleport_cooldown;
    private bool moveable = false;
    private Vector2 mouseLook, joystickLook;
    private Vector3 movement, rotationTarget;
    
    public bool isPc;
    public int coinsCollected = 10;
   
    private int numTurrets = 2;
    [SerializeField]
    private GameObject[] turretPrefabs;
    private GameObject selectedTurretPrefab;
    private GameObject currentPlaceableTurret;
    private float mouseWheelRotation;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private ObjectLifetime dashCooldownTimer;
    [SerializeField] private int dashCooldown;

    public ParticleSystem dashParticles;


    private void Start(){
        moveable = false;
        selectedTurretPrefab  = turretPrefabs[0];
        if(health_bar) health_bar.SetMaxHealth(gameObject.GetComponent<Health>().health);
    }

    public void OnMove(InputAction.CallbackContext context){
        Vector2 input = context.ReadValue<Vector2>();
        movement = Vector3.Normalize(new Vector3(input.x, 0f, input.y));
    }

    public void OnShoot(InputAction.CallbackContext context){
        if(currentPlaceableTurret) {
            if(context.phase == InputActionPhase.Performed) {
                SoundManager.Instance.PlaySFX("Turret Placement");
                // We have to en-/disable raycasting for turrets as otherwise
                // it would mess with the placement of the turret
                // (it is already shown on the scene so the ray will hit the turret even though it isn't placed)
                coinsCollected -= currentPlaceableTurret.GetComponent<Turret>().GetCost();
                currentPlaceableTurret = null;
            }
            return;
        } else {
            if(context.phase == InputActionPhase.Performed) {
                // TOOD sound?
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                var layerMask = 1 << LayerMask.NameToLayer("Turret");
                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask)){
                    GameObject hit = hitInfo.transform.gameObject;
                    if(hit.tag != "Turret")
                        return;
                    currentPlaceableTurret = hit;
                    coinsCollected += currentPlaceableTurret.GetComponent<Turret>().GetCost();
                }
            }
        }

    }

    public void OnDash(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Started && dashCooldownTimer == null){
            CharacterController cct = gameObject.GetComponent<CharacterController>();
            cct.enabled = false;
            // This does not handle moving through an object
            // e.g. a single enemy can block as from teleporting past him.
            // cct.Move(movement * dashDistance);
            transform.Translate(movement * dashDistance, Space.World);
            cct.enabled = true;
            dashCooldownTimer = gameObject.AddComponent(typeof(ObjectLifetime)) as ObjectLifetime;
            dashCooldownTimer.destroyGameObject = false;
            teleport_cooldown.SetCooldown(dashCooldownTimer);
            SoundManager.Instance.PlaySFX("Dash");
            dashParticles.Play();
        }
    }
    
    public void OnMouseLook(InputAction.CallbackContext context){
        mouseLook = context.ReadValue<Vector2>();
    }
    
    public void OnJoystickLook(InputAction.CallbackContext context){
        joystickLook = context.ReadValue<Vector2>();
    }
    
    private void Update(){
        if(currentPlaceableTurret != null){
            MoveCurrentPlaceableTurretToMouse();
            RotateFromMouseWheel();
        }
        if(moveable){
            if(isPc){
                RaycastHit hit;
                var layerMask = 1 << LayerMask.NameToLayer("Default");
                Ray ray = Camera.main.ScreenPointToRay(mouseLook);

                if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
                    rotationTarget = hit.point;
                }

                movePlayerWithAim();
            } else {
                if(joystickLook.x == 0 && joystickLook.y == 0) {
                    movePlayer();
                }else{
                    movePlayerWithAim();
                }
            }
        }
    }

    private void movePlayer(){
        if(movement != Vector3.zero){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
        }
        gameObject.GetComponent<CharacterController>().Move(movement * moveSpeed * Time.deltaTime);
    }

    public void movePlayerWithAim(){
        if(isPc){
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if(aimDirection != Vector3.zero){
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
            }
        } else {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);

            if(aimDirection != Vector3.zero){
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), rotationSpeed);
            }
        }

        gameObject.GetComponent<CharacterController>().Move(movement * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        GameObject target = other.gameObject;
        if (target.tag is "Enemy" or "EnemyBullet" or "Explosion" or "aoe_dmg") {
            switch (GetComponent<Health>().TakeDamage(1)) {
                case HitResult.Invuln:
                    break;
                case HitResult.Hit:
                    SoundManager.Instance.PlaySFX("Player Hurt");
                    break;
                case HitResult.Dead:
                    GameObject.FindWithTag("Manager").GetComponent<GameMode>().Loose();
                    break;
            }
            return;
        }
        if(target.tag == "Coin") {
            Destroy(target);
            //GameObject.FindWithTag("Manager").GetComponent<GameMode>().collectCoin();
            coinsCollected += target.GetComponent<LootData>().getValue();
            SoundManager.Instance.PlaySFX("Coin Pickup");
        }
    }

    private void MoveCurrentPlaceableTurretToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        var layerMask = 1 << LayerMask.NameToLayer("Default");
        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask)){
            currentPlaceableTurret.transform.position = hitInfo.point;
            // We want the turret to rotate freely (e.g. mousewheel)
            // currentPlaceableTurret.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation = Input.mouseScrollDelta.y;
        currentPlaceableTurret.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    public void OnPicking(InputAction.CallbackContext context){
        if(context.phase != InputActionPhase.Performed)
            return;
        if(currentPlaceableTurret == null){
            if(selectedTurretPrefab.GetComponent<Turret>().GetCost() <= coinsCollected){
                currentPlaceableTurret = Instantiate(selectedTurretPrefab);
            }
        }
        else if(currentPlaceableTurret != null){
            Destroy(currentPlaceableTurret);
        }
    }

    public void OnSelectTurret(InputAction.CallbackContext context){
        string pressedKey = context.control.ToString();
        char pressedKey_char = pressedKey[pressedKey.Length - 1];
        int turretIndex = pressedKey_char - '0';
        if(turretPrefabs[turretIndex-1].GetComponent<Turret>().GetCost() <= coinsCollected){
            selectedTurretPrefab = turretPrefabs[turretIndex-1];

            if(currentPlaceableTurret!=null){
                Destroy(currentPlaceableTurret);
                currentPlaceableTurret = Instantiate(selectedTurretPrefab);
            }
        }

    }

    public void setMovable(bool value){
        moveable = value;
    }

    public void updateHealthBar(int value){
        health_bar.SetHealth(value);
    }

    public int getCoinsCollected(){
        return coinsCollected;
    }

}
