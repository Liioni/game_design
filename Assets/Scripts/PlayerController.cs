using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    private Vector2 move;

    public void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    private void Start(){
        
    }
    
    private void Update(){
        movePlayer();
    }

    private void movePlayer(){
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}
