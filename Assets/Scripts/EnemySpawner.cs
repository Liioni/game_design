using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public GameObject player, enemy;
    public Enemy enemy;

    public bool active = true;
    [Range(1, 100)]
    public int avgTimeBetweenSpawns = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    void SpawnEnemy() {

        Vector3 pos;
        if(Random.Range(0,2) == 1) {
            if(Random.Range(0,2) == 1) {
                pos = new Vector3(-40, 0, Random.Range(-25,25));
            } else {
                pos = new Vector3(40, 0, Random.Range(-25,25));
            }
        } else {
            if(Random.Range(0,2) == 1) {
                pos = new Vector3(Random.Range(-40,40), 0, -25);
            } else {
                pos = new Vector3(Random.Range(-40,40), 0, 25);
            }
        }
        //GameObject instance = Instantiate(enemy, pos, Quaternion.identity);
        // TODO fix rotation
        // instance.transform.LookAt(player.transform);
        //instance.GetComponent<MoveToObject>().target = player;

        Instantiate(enemy, pos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(active && UnityEngine.Random.Range(0, avgTimeBetweenSpawns) == 0) {
            SpawnEnemy();
        }
    }
}
