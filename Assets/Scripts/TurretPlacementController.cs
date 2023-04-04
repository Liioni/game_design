using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TurretPlacementController : MonoBehaviour
{

    [SerializeField]
    private GameObject turretPrefab;

    private GameObject currentPlaceableTurret;
    private float mouseWheelRotation;
    // Update is called once per frame
    void Update()
    {
        if(currentPlaceableTurret !=null){
            MoveCurrentPlaceableTurretToMouse();
            RotateFromMouseWheel();
        }
    }





    private void MoveCurrentPlaceableTurretToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo)){
            currentPlaceableTurret.transform.position = hitInfo.point;
            currentPlaceableTurret.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }
        private void RotateFromMouseWheel()
    {
        mouseWheelRotation = Input.mouseScrollDelta.y;
        currentPlaceableTurret.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    public void OnPicking(InputAction.CallbackContext context){
        if(currentPlaceableTurret==null && context.phase == InputActionPhase.Performed){
            currentPlaceableTurret = Instantiate(turretPrefab);
        }
        else if(currentPlaceableTurret!=null && context.phase == InputActionPhase.Performed){
            Destroy(currentPlaceableTurret);
        }
    }

    public void OnPlacing(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed) {
            currentPlaceableTurret = null;
        }
    }
}
