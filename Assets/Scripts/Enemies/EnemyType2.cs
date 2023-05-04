using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyType2 : Enemy
{
    public Transform bulletSpawnPoint;
    public GameObject bullet;

    public float speed;
    public float maxDistanceToTarget;
    public float angleSpread;
    public int amountOfBullets;

    private float timer = 0.0f;
    public float bulletInterval;

    // Start is called before the first frame update
    void Start()
    {
        bulletSpawnPoint = transform.Find("BulletSpawnPoint");
        speed = 5;
        maxDistanceToTarget = 15f;
        angleSpread = 90f;
        amountOfBullets = 10;
        bulletInterval = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        chaseAndShootTarget();
    }

    void spawnCircleOfBullets(int numberOfBullets)
    {
        float angleStep = angleSpread / numberOfBullets;
        Quaternion currentRotation = transform.rotation;
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = (-angleSpread / 2.0f) + (i * angleStep);
            GameObject sphere = Instantiate(bullet, transform.position, Quaternion.identity);
            EnemyBullet variables = sphere.GetComponent<EnemyBullet>();
            Vector3 calculatedDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0f, Mathf.Cos(Mathf.Deg2Rad * angle));
            variables.direction = currentRotation * calculatedDirection;
            variables.speed = speed;
            Destroy(sphere, 5.0f);
        }
    }

    void chaseAndShootTarget() {
        timer += Time.deltaTime;
        if (Vector3.Distance(transform.position, target.transform.position) > maxDistanceToTarget)
        {
            ChaseTarget();
        }
        else
        {
            if (timer >= bulletInterval)
            {
                spawnCircleOfBullets(amountOfBullets);
                timer = 0f;
            }
        }
    }
}

//public class Origin : MonoBehaviour
//{
    
//    float timer = 0f;
//    [SerializeField]
//    int x;
//    [SerializeField]
//    public bool circle;
//    [SerializeField]
//    public bool burst;
//    [SerializeField]
//    public float interval;

//    System.Random rnd;

//    // Start is called before the first frame update
//    void Start()
//    {
//        rnd = new System.Random();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (circle)
//        {
//            timer += Time.deltaTime;
//            if (timer > interval)
//            {
//                //int randomInt = rnd.Next(5, 11);
//                spawnCircleOfBullets(x);
//                timer = 0f;
//                transform.Rotate(Vector3.up, 10f);
//            }
//        }

//        if (burst)
//        {
//            timer += Time.deltaTime;
//            if (timer > interval)
//            {
//                for (int i = 0; i < 4; i++)
//                {
//                    Vector3 position = transform.position;
//                    position.x -= i * 0.2f;
//                    spawnBullet(position, new Vector3(0f, 0f, 1f), 10 / (i * 0.5f + 1));
//                }
//                timer = 0f;
//            }
//        }


//    }


//}
