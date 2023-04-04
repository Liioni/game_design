using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public float health;
    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChaseTarget() {
        if (target == null) { return; }
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += movementSpeed * Time.deltaTime * direction;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Damage should be based on some variable within the colliding object
        if (collision.gameObject.CompareTag("Bullet")) {
            TakeDamage(50f);
            //Debug.Log(health);
        }
        if (collision.gameObject.CompareTag("TurretBullet")) {
            TakeDamage(10f);
        }
        Destroy(collision.gameObject);
    }
}
