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
    private Vector2 move, mouseLook, joystickLook;
    private Vector3 rotationTarget;
    
    public bool isPc;
    public bool canShoot = false;
    public int towersAvailable = 1;
    private int towersPlaced = 0;
    [SerializeField]
    private GameObject turretPrefab;
    private GameObject currentPlaceableTurret;
    private float mouseWheelRotation;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private ObjectLifetime dashCooldownTimer;
    [SerializeField] private int dashCooldown;

    public AudioSource shootingSound;
    public AudioSource dashSound;
    public AudioSource hurtSound;
    public AudioSource placingSound;
    public AudioSource coinSound;

    public void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context){
        if(currentPlaceableTurret) {
            if(context.phase == InputActionPhase.Performed) {
                currentPlaceableTurret = null;
                towersPlaced++;
            }
            return;
        }
        if(!canShoot)
            return;
        // Started, Performed, Canceled <-- Which phase is the best to initialize the firing?
        if(context.phase == InputActionPhase.Performed) {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            shootingSound.Play();
        }
    }

    public void OnDash(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Started && dashCooldownTimer == null){
            Vector3 movement = new Vector3(move.x, 0f, move.y);
            
            transform.Translate(Vector3.Normalize(movement) * dashDistance, Space.World);
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


    private void Start(){
    }
    
    private void Update(){
        if(currentPlaceableTurret != null){
            MoveCurrentPlaceableTurretToMouse();
            RotateFromMouseWheel();
        }
        if(isPc){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseLook);

            if(Physics.Raycast(ray, out hit)){
                rotationTarget = hit.point;
            }

            movePlayerWithAim();
        }else{
            if(joystickLook.x == 0 && joystickLook.y == 0) {
                movePlayer();
            }else{
                movePlayerWithAim();
            }
        }
    }

    private void movePlayer(){
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if(movement != Vector3.zero){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
        }

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    public void movePlayerWithAim(){
        if(isPc){
            var lookPos= rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if(aimDirection != Vector3.zero){
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
            }
        }else{
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);

            if(aimDirection != Vector3.zero){
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), rotationSpeed);
            }
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
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
            GameObject.FindWithTag("Manager").GetComponent<GameMode>().incrementScore();
            coinSound.Play();
        }
    }

    private void MoveCurrentPlaceableTurretToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo)){
            currentPlaceableTurret.transform.position = hitInfo.point;
            currentPlaceableTurret.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            placingSound.Play();
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation = Input.mouseScrollDelta.y;
        currentPlaceableTurret.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    public void OnPicking(InputAction.CallbackContext context){
        if(towersAvailable - towersPlaced > 0 && currentPlaceableTurret == null && context.phase == InputActionPhase.Performed){
            currentPlaceableTurret = Instantiate(turretPrefab);
            currentPlaceableTurret.GetComponent<Turret>().burstSize = 2 + towersAvailable;
        }
        else if(currentPlaceableTurret != null && context.phase == InputActionPhase.Performed){
            Destroy(currentPlaceableTurret);
        }
    }
}
