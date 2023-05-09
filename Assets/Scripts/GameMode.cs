using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    UI ui;
    private int startMenuScene = 0;
    public bool towerMode;
    private bool _activeWave = false;
    private bool _paused = false;
    public int waveNumber = 0;
    public int coinsCollected = 0;
    public int coinsNeeded;
    public ObjectLifetime timer;

    public List<Spawner> waveSpawners;

    public AudioSource waveSoundtrack;
    public AudioSource mainSoundtrack;

    private GameObject[] enemies;
    private GameObject[] turrets;
    private AudioSource menuSoundtrack;
    void setActiveWave(bool newVal) { 
        _activeWave = newVal;
        activateScripts(newVal);
        if(newVal){
            if(menuSoundtrack.isPlaying) 
                menuSoundtrack.Stop();
            mainSoundtrack.Stop();
            waveSoundtrack.Play();
            waveNumber++;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().flipMovable(newVal);
        }
        if(!newVal) {
            waveSoundtrack.Stop();
            if(!menuSoundtrack.isPlaying) 
                mainSoundtrack.Play();
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().flipMovable(!newVal);
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
        if(newVal) {
            if(timer) Destroy(timer);
            timer = gameObject.AddComponent(typeof(ObjectLifetime)) as ObjectLifetime;
            timer.destroyGameObject = false;
            timer.life_span = 30;
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
        if(GameObject.FindWithTag("Player")) GameObject.FindWithTag("Player").GetComponent<PlayerController>().flipMovable(!_paused);
        if(_paused){
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            turrets = GameObject.FindGameObjectsWithTag("Turret");
            pauseGame();
            timer.setPause(true);
        }else{
            pauseGame();
            enemies = null;
            turrets = null;
            timer.setPause(false);
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
        menuSoundtrack = (AudioSource)GameObject.FindGameObjectWithTag("Soundtrack").GetComponent<AudioSource>();
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
        SceneManager.LoadScene(startMenuScene);
    }

    void Update() {
        if(!timer && _activeWave) {
            setActiveWave(false);
            increaseDifficulty();
        }
    }
}
