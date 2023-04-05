using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float playerHealth = 100f;
    public Rigidbody rb;
    public float invincibilityTime = 1f;
    private float invincibilityTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
         rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityTimer > 0f)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        if (playerHealth <= 0) {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && invincibilityTimer <= 0f) {
            playerTakeDamage(50f, collision);
            //Debug.Log(playerHealth);
        }
    }

    public void playerTakeDamage(float damage, Collision collision) {
        playerHealth -= damage;
        invincibilityTimer = invincibilityTime;
        //Vector3 pushbackDirection = collision.contacts[0].normal;
        //rb.AddForce(pushbackDirection.normalized * 5f, ForceMode.Impulse);
    }
}
