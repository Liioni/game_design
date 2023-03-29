using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public float range = 15f;

    public string enemyTag = "Enemy";

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
            target = nearestEnemy.transform;
        }else{
            target = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
            return; 
            
        
    }


    //the range is only drawn when the target is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, range); //params: position, radius

    }
}
