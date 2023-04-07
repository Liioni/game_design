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
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI turretsText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text ="Score: "+ gamemode.score.ToString();

        string? waveStatus = spawner.active ? "active"  : "inactive";
        waveText.text = "Wave: " + waveStatus;

        turretsText.text = "Turrets: " + playerController.towersAvailable.ToString();
    }
}
