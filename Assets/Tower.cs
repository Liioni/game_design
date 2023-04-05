using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour
{
    public float towerHealth = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (towerHealth <= 0) {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            towerTakeDamage(50f);
            Destroy(collision.gameObject);
            Debug.Log(towerHealth);
        }
    }

    public void towerTakeDamage(float damage) {
        towerHealth -= damage;
    }
}
