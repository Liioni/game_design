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
    private bool moveable = false;
    private Vector2 mouseLook, joystickLook;
    private Vector3 movement, rotationTarget;
    
    public bool isPc;
    public int towersAvailable = 1;
    public int towersPlaced = 0;
   
    [SerializeField]
    private GameObject[] turretPrefabs;
    private int turretIndex = 0;
    private GameObject currentPlaceableTurret;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private ObjectLifetime dashCooldownTimer;
    [SerializeField] private int dashCooldown;

    public AudioSource dashSound;
    public AudioSource hurtSound;
    public AudioSource placingSound;
    public AudioSource coinSound;
    

    private void Start(){
        moveable = false;
    }

    public void OnMove(InputAction.CallbackContext context){
        Vector2 input = context.ReadValue<Vector2>();
        movement = Vector3.Normalize(new Vector3(input.x, 0f, input.y));
    }

    public void OnShoot(InputAction.CallbackContext context){
        if(currentPlaceableTurret) {
            if(context.phase == InputActionPhase.Performed) {
                placingSound.Play();
                // We have to en-/disable raycasting for turrets as otherwise
                // it would mess with the placement of the turret
                // (it is already shown on the scene so the ray will hit the turret even though it isn't placed)
                currentPlaceableTurret = null;
                towersPlaced++;
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
                    towersPlaced--;
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
            dashSound.Play();
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
        if (target.tag == "Enemy") {
            switch (GetComponent<Health>().TakeDamage(1)) {
                case HitResult.Invuln:
                    break;
                case HitResult.Hit:
                    hurtSound.Play();
                    break;
                case HitResult.Dead:
                    GameObject.FindWithTag("Manager").GetComponent<GameMode>().Loose();
                    break;
            }
            return;
        }
        if(target.tag == "Coin") {
            Destroy(target);
            GameObject.FindWithTag("Manager").GetComponent<GameMode>().collectCoin();
            coinSound.Play();
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

    private void UpdateSelected() {
        if(currentPlaceableTurret == null) {
            if(towersAvailable <= towersPlaced) {
                return;
            }
        } else {
            Destroy(currentPlaceableTurret);
        }

        currentPlaceableTurret = Instantiate(turretPrefabs[turretIndex]);
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if(context.phase != InputActionPhase.Performed) {
            return;
        }
        if(currentPlaceableTurret == null) {
            return;
        }
        float scroll = context.ReadValue<float>();
        if(scroll > 0) {
            turretIndex += 1;
        } else if(scroll < 0) {
            turretIndex -= 1;
        }
        // C# modulo does not handle negative numbers correctly.
        // Adding length first, then modulo, ensures that only positive numbers are returned
        turretIndex = (turretIndex + turretPrefabs.Length) % turretPrefabs.Length;

        UpdateSelected();
    }

    public void OnPicking(InputAction.CallbackContext context){
        if(context.phase != InputActionPhase.Performed)
            return;
        if(currentPlaceableTurret == null) {
            UpdateSelected();
        } else {
            Destroy(currentPlaceableTurret);
        }
    }

    public void OnSelectTurret(InputAction.CallbackContext context){
        string pressedKey = context.control.ToString();
        char pressedKey_char = pressedKey[pressedKey.Length - 1];
        turretIndex = pressedKey_char - '0' - 1;

        UpdateSelected();
    }

    public void addAvailableTowers(int value){
        towersAvailable += value;
    }

    public void setMovable(bool value){
        moveable = value;
    }
}
