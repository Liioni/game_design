using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LookAtTarget();
        ChaseTarget();
    }
}
