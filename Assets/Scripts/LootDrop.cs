using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject[] loot;
    [SerializeField]
    private float lootFrequency;

    public void DropLoot(Vector3 pos){
        if(Random.value <= lootFrequency && loot.Length > 0){
            //Debug.Log(loot[Random.Range(0, loot.Length)]);   
            Instantiate(loot[Random.Range(0, loot.Length)], pos, Quaternion.identity);
        }      
    }
}
