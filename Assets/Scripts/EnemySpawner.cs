using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy, coin;

    public bool active = true;
    [Range(1, 100)]
    public int avgTimeBetweenEnemies = 50;
    [Range(1, 100)]
    public int timeBetweenCoins = 10;

    private float coinCooldown = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void setActive(bool isActive){
        active = isActive;
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

    void SpawnEnemy(bool borderOnly) {
        Vector3 pos;
        if(borderOnly) {
            pos = SpawnOnBorder();
        } else {
            pos = SpawnEverywhere();
        }
        Instantiate(enemy, pos, Quaternion.identity);
    }

    void SpawnCoin() {
        Vector3 pos = SpawnEverywhere();
        GameObject instance = Instantiate(coin, pos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(active && UnityEngine.Random.Range(0, avgTimeBetweenEnemies) == 0) {
            SpawnEnemy(true);
        }

        coinCooldown -= Time.deltaTime;
        if(active && coinCooldown <= 0) {
            SpawnCoin();
            coinCooldown = timeBetweenCoins;
        }
    }
}
