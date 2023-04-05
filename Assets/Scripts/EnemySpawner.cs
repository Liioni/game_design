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

    public void setActive(bool isActive){
        active= isActive;
    }

    void SpawnEnemy() {

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
        Instantiate(enemy, pos, Quaternion.identity);
        //instance.GetComponent<MoveToObject>().target = player;
    }

    // Update is called once per frame
    void Update()
    {
        if(active && UnityEngine.Random.Range(0, avgTimeBetweenSpawns) == 0) {
            SpawnEnemy();
        }
    }
}
