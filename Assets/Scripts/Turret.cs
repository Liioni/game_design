using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject target;

    [Header ("Attributes")]
    public float range = 15f;
    public float fireRate = 0.5f; // bursts/second
    public float burstSize = 3;
    private float fireCooldown = 0f;
    private float burstCount = 0;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public float turnSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    GameObject GetClosest(GameObject[] enemies) {
        //look for the closest enemy - initialize to infinity
        float shortestDistance = Mathf.Infinity;
        // Guaranteed to be non-null afterwards as size > 0
        GameObject target = null;
        foreach(GameObject enemy in enemies) {
            //store distance of each enemy
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance){
                shortestDistance = distanceToEnemy;
                target = enemy;
            }
        }
        if(shortestDistance > range)
            target = null;
        return target;
    }
    //
    //repetitive function that is not called in every frame (too heavy) to find turret targets
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = GetClosest(enemies);

        target = nearestEnemy;
    }


    // Returns relative angle remaining between target and self
    float Rotate(GameObject target) {
        //direction is vector that goes from us to target
        Vector3 direction = target.transform.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //only rotate around y axis
        float angle_abs = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime*turnSpeed).eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0f, angle_abs, 0f);
        float angle_rel = Quaternion.Angle(transform.rotation, rotation);
        // Don't turn unless the burst is over. Return angle anyway
        if(burstCount == 0) {
            transform.rotation = rotation;
        }
        return angle_rel;
    }

    void Shoot(GameObject target, float angle) {
        if(fireCooldown > 0f || burstCount == 0 && angle > 1) {
            return;
        }

        GameObject instance = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        burstCount++;

        // Two thirds on cooldown, one third on burst.
        // Each shoot in the burst adds 1/9, thus 3/9 for the total burst
        if(burstCount >= burstSize) {
            fireCooldown = 2f/3f * 1f/fireRate;
            burstCount = 0;
        } else {
            fireCooldown = 1f/3f * 1f/fireRate / burstSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if(target == null)
            return;

        float angle = Rotate(target);

        Shoot(target, angle);
    }

    //the range is only drawn when the target is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, range); //params: position, radius

    }
}
