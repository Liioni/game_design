using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance;
   public Sound[] musicSounds, sfxSounds;
   public AudioSource musicSource, sfxSource;

   private void Awake(){
      if(Instance==null){
         Instance=this;
         DontDestroyOnLoad(gameObject);
      }
      else{
         Destroy(gameObject);
      }
   }

   private void Start(){
      PlayMusic("Main Theme");
   }

   public void PlayMusic(string name){
      Sound s = Array.Find(musicSounds, x=>x.name==name);

      if(s==null){
         Debug.Log("Sound not found");
      }
      else{
         musicSource.clip=s.clip;
         musicSource.Play();
      }
   }

   public void PlaySFX(string name){
      Sound s = Array.Find(sfxSounds, x=>x.name==name);

      if(s==null){
         Debug.Log("Sound not found");
      }
      else{
         sfxSource.PlayOneShot(s.clip);
      }
   }

   public void ChangeMusicVolume(float volume){
         musicSource.volume = volume;
   }

   public void ChangeSFXVolume(float volume){
         sfxSource.volume = volume;
   }
   
}
