using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    public void  MusicVolume(){
        SoundManager.Instance.ChangeMusicVolume(_musicSlider.value);
    }

    public void SFXVolume(){
        SoundManager.Instance.ChangeSFXVolume(_sfxSlider.value);
    }
}
