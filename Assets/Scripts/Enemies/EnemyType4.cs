using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType4 : Enemy
{
    public GameObject fallingSphere;

    private float xLow;
    private float xHigh;
    private float zLow;
    private float zHigh;

    private float timer = 0f;
    private float spawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        GameObject groundObject = GameObject.Find("Ground");

        spawnInterval = Random.Range(1f, 4f);

        xLow = groundObject.transform.localScale.x / -2f + 5f;
        xHigh = groundObject.transform.localScale.x / 2f - 5f;
        zLow = groundObject.transform.localScale.z / -2f + 5f;
        zHigh = groundObject.transform.localScale.z / 2f - 5f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval) {
            spawnFallingSphere();
            timer = 0f;
        }
    }

    public void spawnFallingSphere() {
        float x = Random.Range(xLow, xHigh);
        float z = Random.Range(zLow, zHigh);
        Vector3 position = new Vector3(x, 10f, z);
        Instantiate(fallingSphere, position, Quaternion.identity);
    }
}
