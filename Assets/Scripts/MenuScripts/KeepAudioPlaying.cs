using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAudioPlaying : MonoBehaviour
{
    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
}
