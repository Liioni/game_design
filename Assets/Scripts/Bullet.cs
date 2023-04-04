using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float life_span = 1.5f;
    public float initial_speed;
    private Vector3 velocity;
    private bool velocity_set = false;

    ObjectLifetime lifetime;
    Rigidbody rb;

    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start(){
        lifetime = GetComponent<ObjectLifetime>();
        rb = GetComponent<Rigidbody>();
        initial_speed = 15f;
    }

    // Update is called once per frame
    void Update(){
        if(rb != null && !velocity_set){
            rb.velocity = velocity * initial_speed;
            velocity_set = true;
        }
        if(life_span < lifetime.GetElapsedTime()){
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector3 v){
        velocity = v;
    }

    private void OnCollisionEnter(Collision collision)
    { 
        Debug.Log("Collision");
         if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, 2f);

            Destroy(gameObject);
        }

    }
}
