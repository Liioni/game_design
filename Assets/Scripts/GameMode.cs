using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    public bool towerMode;
    private bool _activeWave = false;
    public int score = 0;
    public ObjectLifetime timer;

    public List<Spawner> waveSpawners;

    void setActiveWave(bool newVal) {
        _activeWave = newVal;
        foreach(var script in waveSpawners) {
            script.setActive(newVal);
        }
        if(towerMode && newVal) {
            if(timer) Destroy(timer);
            timer = gameObject.AddComponent(typeof(ObjectLifetime)) as ObjectLifetime;
            timer.destroyGameObject = false;
            timer.life_span = 30;
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
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().towersAvailable++;
            foreach(var script in waveSpawners) {
                script.difficulty++;
            }
        }
        Debug.Log(score);
    }

    void Update() {
        if(!timer && _activeWave) {
            setActiveWave(false);
            incrementScore();
            incrementScore();
            incrementScore();
        }
    }
}
