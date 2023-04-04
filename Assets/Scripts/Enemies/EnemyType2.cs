using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        health = 50f;
        movementSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ChaseTarget();
    }
}
