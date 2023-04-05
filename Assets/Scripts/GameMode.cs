using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    public bool towerMode;
    private bool _activeWave = false;
    public int score = 0;

    public List<Spawner> waveSpawners;

    void setActiveWave(bool newVal) {
        _activeWave = newVal;
        foreach(var script in waveSpawners) {
            script.setActive(newVal);
        }
    }

    public void flipActiveWave() {
        setActiveWave(!_activeWave);
    }

    void Start() {
        // To ensure each spawner is set correctly
        setActiveWave(_activeWave);
    }

    public void incrementScore(){
        score++;
        if(score % 3 == 0) {
            setActiveWave(false);
        }
        Debug.Log(score);
    }
}
