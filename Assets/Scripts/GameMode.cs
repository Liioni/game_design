using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(!newVal) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            foreach(GameObject x in enemies) {
                Destroy(x);
            }
            foreach(GameObject x in coins) {
                Destroy(x);
            }

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

    public void incrementScore() {
        score++;
        if(score % 3 == 0) {
            setActiveWave(false);
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().towersAvailable = score / 3;
            foreach(var script in waveSpawners) {
                script.difficulty++;
            }
        }
        Debug.Log(score);
    }

    public void Loose() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update() {
        if(towerMode && !timer && _activeWave) {
            setActiveWave(false);
            incrementScore();
            incrementScore();
            incrementScore();
        }
    }
}
