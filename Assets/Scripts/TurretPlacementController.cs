using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacementController : MonoBehaviour
{

    [SerializeField]
    private GameObject turretPrefab;

    [SerializeField]
    private KeyCode newTurretHotkey = KeyCode.LeftShift;

    [SerializeField]
    private KeyCode releaseTurretHotkey = KeyCode.LeftControl;

    private GameObject currentPlaceableTurret;
    private float mouseWheelRotation;
    // Update is called once per frame
    void Update()
    {
        HandleNewTurretHotkey();   

        if(currentPlaceableTurret !=null){
            MoveCurrentPlaceableTurretToMouse();
            RotateFromMouseWheel();
            CheckRelease();
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

    private void HandleNewTurretHotkey()
    {
        if(Input.GetKeyDown(newTurretHotkey)){
            if(currentPlaceableTurret==null){
                currentPlaceableTurret = Instantiate(turretPrefab);
            }
            else{
                Destroy(currentPlaceableTurret);
            }
                
        }
    }

    private void CheckRelease()
    {
        if(Input.GetKeyDown(releaseTurretHotkey)){
            currentPlaceableTurret = null;
        }
    }
}
