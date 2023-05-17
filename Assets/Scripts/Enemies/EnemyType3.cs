using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3 : Enemy
{
    public GameObject explosion;

    private float timer;
    private bool flag;
    private float maxDistanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = 6f;
        timer = 1f;
        maxDistanceToTarget = 5f;
        flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtTarget();
        ChaseTarget();
        if (Vector3.Distance(transform.position, target.transform.position) <= maxDistanceToTarget) {
            flag = true;
        }
        if (flag) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                BlowUp();
            }
        }
    }

    public void BlowUp() {
        GameObject tempExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(tempExplosion, 1f);
        Destroy(gameObject);
    }
}
