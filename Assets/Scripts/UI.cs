using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField]
    GameMode gamemode; 
    [SerializeField]
    Spawner spawner;
    [SerializeField]
    PlayerController playerController; 
    [SerializeField]
    RawImage image;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI turretsText;

    // Update is called once per frame
    void Update(){
        string waveStatus = spawner.active ? "active"  : "inactive";
        waveText.text = "Wave " + gamemode.waveNumber.ToString() + "\n" + waveStatus;

        turretsText.text = playerController.towersPlaced.ToString() + "/" + playerController.towersAvailable.ToString();
    }

    public void setButtonsActive(bool value){
        image.gameObject.SetActive(value);
    }
}
