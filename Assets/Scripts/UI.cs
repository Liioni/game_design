using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField]
    private GameMode gamemode; 
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private PlayerController playerController; 
    [SerializeField]
    private RawImage waveImage;
    [SerializeField]
    private Canvas pauseMenu;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI turretsText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timerText;

    // Update is called once per frame
    void Update(){
        string waveStatus = spawner.active ? "active"  : "inactive";
        waveText.text = "Wave " + gamemode.waveNumber.ToString();

        turretsText.text = playerController.towersPlaced.ToString() + "/" + playerController.towersAvailable.ToString();

        coinsText.text = gamemode.coinsCollected.ToString();

        if(gamemode.timer) timerText.text = gamemode.timer.timeLeft().ToString("0.0");
        else timerText.text = "0";
    }

    public void setButtonsActive(bool value){
        waveImage.gameObject.SetActive(value);
    }

    public void setPauseActive(bool value){
        pauseMenu.gameObject.SetActive(value);
    }
}
