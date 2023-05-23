using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum HitResult {
    Invuln,
    Hit,
    Dead,
}

public class Health : MonoBehaviour
{
    public int health = 1;
    [SerializeField]
    private bool isPlayer;
    // public Rigidbody rb;
    public float invulnDuration = 1f;
    private ObjectLifetime invulnTimer;

    IEnumerator Blink() {
        while(invulnTimer) {
            gameObject.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.2f);
            gameObject.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    public HitResult TakeDamage(int damage) {
        if(invulnTimer)
            return HitResult.Invuln;
        health -= damage;
        invulnTimer = gameObject.AddComponent(typeof(ObjectLifetime)) as ObjectLifetime;
        invulnTimer.destroyGameObject = false;
        invulnTimer.life_span = invulnDuration;
        if(isPlayer){
            gameObject.GetComponent<PlayerController>().updateHealthBar(health);
        }
        if (health <= 0) {
            return HitResult.Dead;
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //Vector3 pushbackDirection = collision.contacts[0].normal;
        //rb.AddForce(pushbackDirection.normalized * 5f, ForceMode.Impulse);

        //StartCoroutine(Blink());
        return HitResult.Hit;
    }
}
