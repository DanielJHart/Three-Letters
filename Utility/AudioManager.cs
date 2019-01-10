using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource soundEffect, music;

    [SerializeField]
    private AudioClip good, bad, bestScore, musicClip, ATyped, BTyped, CTyped, blip;

    [SerializeField]
    private Slider soundFXSlider, musicSlider;
    
    // Use this for initialization
    private void Start()
    {
        soundFXSlider.onValueChanged.AddListener(SetSoundFXVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        UpdateVolumes();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void SetSoundFXVolume(float volume)
    {
        GameOptions.EffectsVolume = (int)volume;
        soundEffect.volume = volume / 100;
    }

    public void SetMusicVolume(float volume)
    {
        GameOptions.MusicVolume = (int)volume;
        music.volume = volume / 100;
    }
    
    public void UpdateVolumes()
    {
        soundEffect.volume = GameOptions.EffectsVolume;
        soundFXSlider.value = GameOptions.EffectsVolume;
        music.volume = GameOptions.MusicVolume;
        musicSlider.value = GameOptions.MusicVolume;
    }

    public void PlaySoundEffect(string name)
    {
        ////soundEffect.Stop();

        switch (name)
        {
            case "Good":
                soundEffect.clip = good;
                break;
            case "Bad":
                soundEffect.clip = bad;
                break;
            case "BestScore":
                soundEffect.clip = bestScore;
                break;
            case "ATyped":
                soundEffect.clip = ATyped;
                break;
            case "BTyped":
                soundEffect.clip = BTyped;
                break;
            case "CTyped":
                soundEffect.clip = CTyped;
                break;
            case "Blip":
                soundEffect.clip = blip;
                break;
        }

        soundEffect.Play();
    }
}
