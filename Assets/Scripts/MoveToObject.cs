using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToObject : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 5.0f * Time.deltaTime);
    }
}
