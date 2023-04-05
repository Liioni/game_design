using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initial_speed = 70f;

    public GameObject impactEffect;

    void Start(){
        GetComponent<Rigidbody>().velocity = transform.forward * initial_speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if (target.tag == "Enemy") {
            switch (target.GetComponent<Health>().TakeDamage(1)) {
                case HitResult.Invuln:
                    break;
                case HitResult.Hit:
                    break;
                case HitResult.Dead:
                    Destroy(target);
                    break;
            }
            GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, 1.5f);
            Destroy(gameObject);
        }
    }
}
