using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    UI ui;
    public bool towerMode;
    private bool _activeWave = false;
    public int waveNumber = 0;
    public int coinsCollected = 0;
    public int coinsNeeded;
    public ObjectLifetime timer;

    public List<Spawner> waveSpawners;

    void setActiveWave(bool newVal) {
        _activeWave = newVal;
        foreach(var script in waveSpawners) {
            script.setActive(newVal);
        }
        if(newVal){
            waveNumber++;
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
            ui.setButtonsActive(true);
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
        // To make sure the Buttons show before first wave
        ui.setButtonsActive(true);
    }

    public void collectCoin() {
        coinsCollected++;
        if(coinsCollected % coinsNeeded == 0) {
            setActiveWave(false);
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().towersAvailable = 1 + coinsCollected / coinsNeeded;
            foreach(var script in waveSpawners) {
                script.difficulty++;
            }
        }
        Debug.Log(coinsCollected);
    }

    public void Loose() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update() {
        if(towerMode && !timer && _activeWave) {
            setActiveWave(false);
            collectCoin();
            collectCoin();
            collectCoin();
        }
    }
}
