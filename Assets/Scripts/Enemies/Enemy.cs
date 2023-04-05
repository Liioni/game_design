using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public float health;
    public float movementSpeed;
    public int points;

    public GameMode gameMode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        gameMode = GameObject.FindObjectOfType<GameMode>();
        if (gameMode.towerMode) {
            target = GameObject.FindGameObjectWithTag("Tower");
        } else {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChaseTarget() {
        if (target == null) { return; }
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0f;
        transform.position += movementSpeed * Time.deltaTime * direction;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject);
            // GameObject.FindGameObjectWithTag("Manager").GetComponent<GameMode>().incrementKillCount();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Damage should be based on some variable within the colliding object
        if (collision.gameObject.CompareTag("Bullet")) {
            TakeDamage(100f);
            Destroy(collision.gameObject);
            //Debug.Log(health);
        }
        if (collision.gameObject.CompareTag("TurretBullet")) {
            TakeDamage(50f);
            Destroy(collision.gameObject);
        }
    }
}
