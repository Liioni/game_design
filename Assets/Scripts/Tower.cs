using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour
{
    public AudioSource damageSound;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            switch (GetComponent<Health>().TakeDamage(1)) {
                case HitResult.Invuln:
                    break;
                case HitResult.Hit:
                    damageSound.Play();
                    Destroy(collider.gameObject);
                    break;
                case HitResult.Dead:
                    GameObject.FindWithTag("Manager").GetComponent<GameMode>().Loose();
                    break;
            };
        }
    }
}
