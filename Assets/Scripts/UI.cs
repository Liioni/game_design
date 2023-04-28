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

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI turretsText;
    public TextMeshProUGUI coinsText;

    // Update is called once per frame
    void Update(){
        string waveStatus = spawner.active ? "active"  : "inactive";
        waveText.text = "Wave " + gamemode.waveNumber.ToString();

        turretsText.text = playerController.towersPlaced.ToString() + "/" + playerController.towersAvailable.ToString();
        
        coinsText.text = gamemode.coinsCollected.ToString();
    }

    public void setButtonsActive(bool value){
        waveImage.gameObject.SetActive(value);
    }
}
