using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneraFollow : MonoBehaviour
{
    public GameObject player;
    public Transform target;
    public Transform mainCamera;

    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private float dMin = 0f;
    private float dMax = 100f;

    private float xAngleMin = 0f;
    private float xAngleMax = 90f;

    [Range(0f, 100f)]
    public float d = 50f;

    [Range(0f, 90f)]
    public float xAngle = 90f;

    private float sensitivity = 10f;

    // Start is called before the first frame update
    void Start(){
        mainCamera = GameObject.Find("CameraHolder").transform.Find("Main Camera");
        target = player.transform;
        
        d = 35f;
        xAngle = 65f;
    }

    // Update is called once per frame
    void LateUpdate(){
        if(target != null){

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Input.GetKey(KeyCode.LeftControl)) {
                xAngle -= scroll * sensitivity;
            } else {
                d -= scroll * sensitivity;
            }

            d = Mathf.Clamp(d, dMin, dMax);
            xAngle = Mathf.Clamp(xAngle, xAngleMin, xAngleMax);

            Quaternion targetRotation = Quaternion.Euler(xAngle, 0f, 0f);

            float y = Mathf.Sin(xAngle * Mathf.Deg2Rad) * d;
            float z = Mathf.Cos(xAngle * Mathf.Deg2Rad) * d;

            Vector3 offset = new Vector3(0, y - target.transform.position.y, -z);
            Vector3 targetPosition = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, smoothTime);
        }
    }
}
