using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    public static MusicManager Instance { get; private set; }
    public AudioSource activeAudio;

    public AudioClip[] music;
    public AudioClip[] sounds;

    private AudioClip soundClip;
    private AudioClip musicClip;

    private void Start() {

        if (Instance != null) {
            Debug.LogError("More than one MusicManager instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        if(SceneManager.GetActiveScene().name == "MainMenu") {

            SetAndPlayMusic("MainMenu");

        }
    }

    public void SetAndPlayMusic(string name) {

        switch(name) {

            case "MainMenu":
                musicClip = music[0];
                break;
            case "InGame":
                musicClip = music[1];
                break;
        }

        activeAudio.clip = musicClip;
        activeAudio.Play();

    }

    public void SetAndPlaySound(string name) {

        switch(name){

            case "ButtonSelect":
                soundClip = sounds[0];
                break;
            case "Crank":
                soundClip = sounds[1];
                break;
            case "EnemyDeath":
                soundClip = sounds[2];
                break;
            case "GearDown":
                soundClip = sounds[3];
                break;
            case "Jump":
                soundClip = sounds[4];
                break;
            case "Spawn":
                soundClip = sounds[5];
                break;
            case "PlayerDeath":
                soundClip = sounds[6];
                break;
            case "TickDown":
                soundClip = sounds[7];
                break;
            case "RoundStart":
                soundClip = sounds[8];
                break;
        }

        activeAudio.PlayOneShot(soundClip);

    }

    public void Stop() {

        activeAudio.Stop();

    }

}
