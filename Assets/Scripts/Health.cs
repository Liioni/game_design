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
    // public Rigidbody rb;
    public float invulnDuration = 1f;
    private ObjectLifetime invulnTimer;

    public HitResult TakeDamage(int damage) {
        if(invulnTimer)
            return HitResult.Invuln;
        health -= damage;
        invulnTimer = gameObject.AddComponent(typeof(ObjectLifetime)) as ObjectLifetime;
        invulnTimer.destroyGameObject = false;
        invulnTimer.life_span = invulnDuration;
        if (health <= 0) {
            return HitResult.Dead;
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //Vector3 pushbackDirection = collision.contacts[0].normal;
        //rb.AddForce(pushbackDirection.normalized * 5f, ForceMode.Impulse);
        return HitResult.Hit;
    }
}
