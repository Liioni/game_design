using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    UI ui;
    public float scaling_factor = 1.5f;
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
    private float[] waveLength;
    private float maxWaveLength = 60f;

    public GameObject castlePrefab;

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
            if(timer) Destroy(timer);
            timer = gameObject.AddComponent(typeof(ObjectLifetime)) as ObjectLifetime;
            timer.destroyGameObject = false;
            if (waveNumber > waveLength.Length) {
                timer.life_span = maxWaveLength;
            }
            else {
                timer.life_span = waveLength[waveNumber - 1];
            }
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
            if(waveNumber == 3 && !towerMode) {
              GameObject castle = Instantiate(castlePrefab, new Vector3(0,0,0), Quaternion.identity);
            }
            foreach(var script in waveSpawners) {
              script.difficulty++;
            }
            if(timer) Destroy(timer);
        }
    }

    private void activateScripts(bool value){
        foreach(var script in waveSpawners) {
            script.setActive(value);
            script.scaling_factor = scaling_factor;
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
        waveLength = new float[] { 30f, 30f, 45f, 45f};
    }

    public void collectCoin() {
        coinsCollected++;
    }

    public void increaseDifficulty(){
      setActiveWave(true);
      setActiveWave(false);
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
            GameObject.FindWithTag("Player").GetComponent<Health>().ResetHealth();
        }
    }
}
