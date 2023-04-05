using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public bool active = true;
    public bool onBorder = true;
    [Range(0.01f, 100.0f)]
    public float timeBetweenSpawns = 1.0f;
    private float _cooldown;

    public bool scalesWithDifficulty;
    public int difficulty = 0;

    // Start is called before the first frame update
    void Start()
    {
        // resetCooldown();
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

    private void addCooldown() {
        float modifier = timeBetweenSpawns;
        if(scalesWithDifficulty) {
            modifier *= Mathf.Pow(2.0f, -difficulty);
        }
        _cooldown += modifier;
    }

    // Update is called once per frame
    void Update()
    {
        if(!active)
            return;
        _cooldown -= Time.deltaTime;
        while(_cooldown <= 0) {
            Spawn();
            addCooldown();
        }
    }
}