using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public GameObject impactEffect;

    public void Chase(Transform _target){
        //here we cann also add stuff like pass a damage value to the bullet
        target = _target;
    }
    // Update is called once per frame
    void Update()
    {
        //if bullet loses its target, destroy it
        if(target == null){
            Destroy(gameObject);
            return;
        }

        Vector3 dir= target.position - transform.position;
        
        //distance that we're going to move this frame
        float frameDistance = speed*Time.deltaTime;

        //magnitude= length of direction vector = distance between bullet and target
        //this condition makes sure that we don't overshoot: if magnitude > framedist, it means we've already hit the target
        if(dir.magnitude <= frameDistance){
            HitTarget();
            return;
        } 

        //if we got here, we still haven't hit our target -> bullet moves
        transform.Translate(dir.normalized*frameDistance, Space.World);
    }

    void HitTarget(){
        GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effect, 2f);

        //TODO: damage enemy
        Destroy(gameObject);
        
    }
}
