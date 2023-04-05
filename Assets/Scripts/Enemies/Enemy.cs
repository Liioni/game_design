using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target;
    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        var gameMode = GameObject.FindObjectOfType<GameMode>();
        if (gameMode.towerMode) {
            target = GameObject.FindGameObjectWithTag("Tower");
        } else {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChaseTarget() {
        if (target == null) { return; }
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0f;
        transform.position += movementSpeed * Time.deltaTime * direction;
    }
}
