using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject target;
  
    [Header ("Attributes")]
    public float range = 15f;
    public float fireRate = 1f; // bullets/second
    private float fireCountdown = 0f;

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

    //repetitive function that is not called in every frame (too heavy) to find turret targets
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        //look for the closest enemy - initialize to infinity
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach(GameObject enemy in enemies){
            //store distance of each enemy
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance){
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //we found an enemy and it's in the turret's range
        if(nearestEnemy != null && shortestDistance <=range){
            target = nearestEnemy;
        } else {
            target = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Always reduce cooldown
        // Look at target if it exists
        // Shoot if already looking at it

        fireCountdown -= Time.deltaTime;

        if(target == null)
            return;

        //direction is vector that goes from us to target
        Vector3 direction = target.transform.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //only rotate around y axis
        float angle_abs = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime*turnSpeed).eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0f, angle_abs, 0f);
        float angle_rel = Quaternion.Angle(transform.rotation, rotation);
        transform.rotation = rotation;

        if(fireCountdown > 0f)
            return;
        if(angle_rel < 1) {
            Shoot();
            fireCountdown = 1f/fireRate;
        }
    }

    void Shoot(){
        GameObject instance = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        instance.GetComponent<TurretBullet>().target = target;
    }


    //the range is only drawn when the target is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, range); //params: position, radius

    }
}
