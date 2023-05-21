using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initial_speed = 70f;
    public GameObject impactEffect;
    public GameObject target;

    void Start(){
        if(target==null)
            GetComponent<Rigidbody>().velocity = transform.forward * initial_speed;
    }

    void Update()
    {
        if(target!=null)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, initial_speed * Time.deltaTime);
        else
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
            if(target.GetComponent<LootDrop>()) target.GetComponent<LootDrop>().DropLoot(transform.position);
            GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, 1.5f);
            Destroy(gameObject);
        }
    }
}
