using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyType2 : Enemy
{
    public Transform bulletSpawnPoint;
    public GameObject bullet;

    private float speed;
    private float maxDistanceToTarget;
    private float angleSpread;
    private int amountOfBullets;

    private float timer;
    private float bulletInterval;

    // Start is called before the first frame update
    void Start()
    {
        bulletSpawnPoint = transform.Find("BulletSpawnPoint");

        // Bullet behaviour
        speed = 4f;
        amountOfBullets = 3;
        bulletInterval = 4.0f;
        angleSpread = 60f;

        maxDistanceToTarget = 15f;
        timer = bulletInterval;
    }

    // Update is called once per frame
    void Update()
    {
        ChaseAndShootTarget();
    }

    void SpawnCircleOfBullets(int numberOfBullets)
    {
        float angleStep = angleSpread / numberOfBullets;
        float startAngle = -angleSpread / 2.0f;
        if (numberOfBullets % 2 == 1)
        {
            startAngle += angleStep / 2.0f;
        }
        Quaternion currentRotation = transform.rotation;
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = startAngle + (i * angleStep);
            GameObject sphere = Instantiate(bullet, transform.position, Quaternion.identity);
            EnemyBullet variables = sphere.GetComponent<EnemyBullet>();
            Vector3 calculatedDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0f, Mathf.Cos(Mathf.Deg2Rad * angle));
            variables.direction = currentRotation * calculatedDirection;
            variables.speed = speed;
            Destroy(sphere, 5.0f);
        }
    }

    void ChaseAndShootTarget() {
        LookAtTarget();
        timer += Time.deltaTime;
        if (Vector3.Distance(transform.position, target.transform.position) > maxDistanceToTarget)
        {
            ChaseTarget();
        }
        else
        {
            if (timer >= bulletInterval)
            {
                SpawnCircleOfBullets(amountOfBullets);
                timer = 0f;
                SoundManager.Instance.PlaySFX("Enemy Shoot");
            }
        }
    }
}
