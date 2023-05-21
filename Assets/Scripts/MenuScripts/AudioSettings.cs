using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    void Start(){
        SoundManager.Instance.musicSliders.Add(_musicSlider);
        SoundManager.Instance.sfxSliders.Add(_sfxSlider);
        SoundManager.Instance.LoadValues();
    }
    public void  MusicVolume(){
        SoundManager.Instance.ChangeMusicVolume(_musicSlider.value);
    }

    public void SFXVolume(){
        SoundManager.Instance.ChangeSFXVolume(_sfxSlider.value);
    }
}
