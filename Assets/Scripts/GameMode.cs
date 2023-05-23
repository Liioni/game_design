using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    UI ui;
    private int startMenuScene = 0;
    private int gameOverScene = 4;
    public bool towerMode;
    private bool _activeWave = false;
    private bool _paused = false;
    public int waveNumber = 0;
    public int coinsCollected = 0;
    public int coinsNeeded;
    public ObjectLifetime timer;

    public List<Spawner> waveSpawners;

    private GameObject[] enemies;
    private GameObject[] turrets;
    void setActiveWave(bool newVal) { 
        _activeWave = newVal;
        activateScripts(newVal);
        if(newVal){
            SoundManager.Instance.musicSource.Stop();
            SoundManager.Instance.PlayMusic("Wave Theme");
            waveNumber++;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().setMovable(newVal);
            if(timer) Destroy(timer);
            timer = gameObject.AddComponent(typeof(ObjectLifetime)) as ObjectLifetime;
            timer.destroyGameObject = false;
            timer.life_span = 30;
        } else {
            SoundManager.Instance.musicSource.Stop();
            SoundManager.Instance.PlayMusic("Main Theme");
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().setMovable(!newVal);
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            foreach(GameObject x in enemies) {
                Destroy(x);
            }
            foreach(GameObject x in coins) {
                Destroy(x);
            }
            enemies = null;
            ui.setButtonsActive(true);
        }
    }

    private void activateScripts(bool value){
        foreach(var script in waveSpawners) {
            script.setActive(value);
        }
    }

    public void flipActiveWave() {
        setActiveWave(!_activeWave);
    }

    public void flipPaused(){
        _paused = !_paused;
        ui.setPauseActive(_paused);
        activateScripts(!_paused);
        if(GameObject.FindWithTag("Player")) GameObject.FindWithTag("Player").GetComponent<PlayerController>().setMovable(!_paused);
        if(_paused){
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            turrets = GameObject.FindGameObjectsWithTag("Turret");
            pauseGame();
            if(timer) timer.setPause(true);
        }else{
            pauseGame();
            enemies = null;
            turrets = null;
            if(timer) timer.setPause(false);
        }
    }

    private void pauseGame(){
        Debug.Log("Game Paused");
        activateScripts(!_paused);
        if(enemies != null){
            foreach (var enemy in enemies){
                enemy.gameObject.SetActive(!_paused);
            }
        }
        if(turrets != null){
            foreach (var turret in turrets){
                turret.gameObject.SetActive(!_paused);
            }
        }
    }

    void Start() {
        // To ensure each spawner is set correctly
        setActiveWave(_activeWave);
        // To make sure the Buttons show before first wave
        ui.setButtonsActive(true);
    }

    public void collectCoin() {
        coinsCollected++;
    }

    public void increaseDifficulty(){
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().addAvailableTowers(1);
        foreach(var script in waveSpawners) {
            script.difficulty++;
        }
    }

    public void Loose() {
        SoundManager.Instance.musicSource.Stop();
        SoundManager.Instance.PlaySFX("Game Over");
        SceneManager.LoadScene(gameOverScene);
    }

    void Update() {
        if(!timer && _activeWave) {
            setActiveWave(false);
            increaseDifficulty();
        }
    }
}
