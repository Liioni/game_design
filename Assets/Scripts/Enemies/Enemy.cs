using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<GameObject> targets;
    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
    }

    public void ChaseTarget() {
        targets.Clear();
        foreach(var x in GameObject.FindGameObjectsWithTag("Targetable")) {
            targets.Add(x);
        }
        foreach(var x in GameObject.FindGameObjectsWithTag("Player")) {
            targets.Add(x);
        }

        GameObject closest = null;
        foreach(var x in targets) {
            // This can't be named direction because we use direction outside of the loop? What?
            Vector3 direction2 = x.transform.position - transform.position;

            if(closest == null || direction2.magnitude < (closest.transform.position - transform.position).magnitude) {
                closest = x;
            }
        }
        Vector3 direction = closest.transform.position - transform.position;
        direction.y = 0f;
        direction = direction.normalized;
        gameObject.GetComponent<CharacterController>().Move(direction * movementSpeed * Time.deltaTime);
    }
}
