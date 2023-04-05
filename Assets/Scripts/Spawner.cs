using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public bool active = true;
    public bool onBorder = true;
    public bool random = false;
    [Range(0.01f, 100.0f)]
    public float timeBetweenSpawns = 1.0f;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = timeBetweenSpawns;
    }

    public void setActive(bool newVal) {
        active = newVal;
    }

    Vector3 SpawnOnBorder() {
        Vector3 pos;
        if(Random.Range(0,2) == 1) {
            if(Random.Range(0,2) == 1) {
                pos = new Vector3(-35, 1, Random.Range(-23,23));
            } else {
                pos = new Vector3(35, 1, Random.Range(-23,23));
            }
        } else {
            if(Random.Range(0,2) == 1) {
                pos = new Vector3(Random.Range(-35,35), 1, -23);
            } else {
                pos = new Vector3(Random.Range(-35,35), 1, 23);
            }
        }
        return pos;
    }

    Vector3 SpawnEverywhere() {
        return new Vector3(Random.Range(-35,35), 0, Random.Range(-23,23));
    }

    void Spawn() {
        Vector3 pos;
        if(onBorder) {
            pos = SpawnOnBorder();
        } else {
            pos = SpawnEverywhere();
        }
        Instantiate(prefab, pos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(!active)
            return;
        cooldown -= Time.deltaTime;
        if(random && UnityEngine.Random.Range(0.0f, 1.0f) < 1.0f / timeBetweenSpawns * Time.deltaTime
           || !random && cooldown <= 0) {
            Spawn();
            cooldown = timeBetweenSpawns;
        }
    }
}
