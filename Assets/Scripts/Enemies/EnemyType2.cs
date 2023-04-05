using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ChaseTarget();
    }
}
