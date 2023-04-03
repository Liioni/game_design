using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float dashSpeed;
    private Vector2 move, mouseLook, joystickLook;
    private Vector3 rotationTarget;
    public bool isPc;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private ObjectLifetime dashCooldownTimer;
    [SerializeField] private int dashCooldown;
    private bool dashTimerActive = false;

    public void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context){
        // Started, Performed, Canceled <-- Which phase is the best to initialize the firing?
        if(context.phase == InputActionPhase.Performed) {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null){
                    bulletScript.SetVelocity(bulletSpawn.forward);
                } else{
                    Debug.LogError("Bullet component not found on bullet prefab!");
                }
        }
    }

    public void OnDash(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Started && !dashTimerActive){
            Vector3 movement = new Vector3(move.x, 0f, move.y);

            transform.Translate(movement * dashSpeed * Time.deltaTime, Space.World);
            dashCooldownTimer = gameObject.AddComponent(typeof(ObjectLifetime)) as ObjectLifetime;
            dashTimerActive = true;
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
        if(dashTimerActive){
            if(dashCooldownTimer.GetElapsedTime() > dashCooldown){
                dashTimerActive = false;
                Destroy(dashCooldownTimer);
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

}
