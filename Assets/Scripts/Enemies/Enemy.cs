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
    }

    public void ChaseTarget() {
        if (target == null) { return; }
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0f;
        gameObject.GetComponent<CharacterController>().Move(direction * movementSpeed * Time.deltaTime);
    }

    public void LookAtTarget() {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
