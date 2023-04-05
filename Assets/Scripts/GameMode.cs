using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    public bool towerMode;
    private bool activeWave = false;

    public List<Spawner> waveSpawners;

    void setActiveWave(bool newVal) {
        activeWave = newVal;
        foreach(var script in waveSpawners) {
            script.setActive(newVal);
        }
    }

    public void flipActiveWave() {
        setActiveWave(!activeWave);
    }

    void Start() {
        setActiveWave(activeWave);
    }
}
