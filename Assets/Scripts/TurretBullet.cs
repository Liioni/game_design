using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public float speed = 70f;
    public GameObject impactEffect;

    // Update is called once per frame
    void Update()
    {
        float frameDistance = speed * Time.deltaTime;
        transform.Translate(transform.forward * frameDistance, Space.World);
    }


    void OnTriggerEnter(Collider other){
        GameObject target = other.gameObject;
        if(target.tag != "Enemy")
            return;

        GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effect, 2f);

        Destroy(gameObject);
        Destroy(target);
    }
}
