using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     private float timeToChangeDirection;
 
     // Use this for initialization
     public void Start () {
         ChangeDirection();
     }
     
     // Update is called once per frame
     public void Update () {
         timeToChangeDirection -= Time.deltaTime;
 
         if (timeToChangeDirection <= 0) {
             ChangeDirection();
         }
 
         GetComponent<Rigidbody>().velocity = transform.up * 5;
     }
 

 
     private void ChangeDirection() {
         float angle = Random.Range(0f, 360f);
         Quaternion quat = Quaternion.AngleAxis(angle, Vector3.forward);
         Vector3 newUp = quat * Vector3.up;
         newUp.y = 0;
         newUp.Normalize();
         transform.up = newUp;
         timeToChangeDirection = 1f;
     }
 }