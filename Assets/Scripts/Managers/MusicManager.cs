using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource activeAudio;

    public AudioClip[] music;
    public AudioClip[] sounds;

    private AudioClip clip;

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void SetAndPlaySound(string name) {

        switch(name){

            case "ButtonSelect":
                clip = sounds[0];
                break;
            case "Crank":
                clip = sounds[1];
                break;
            case "EnemyDeath":
                clip = sounds[2];
                break;
            case "GearDown":
                clip = sounds[3];
                break;
            case "Jump":
                clip = sounds[4];
                break;
            case "Spawn":
                clip = sounds[5];
                break;
            case "PlayerDeath":
                clip = sounds[6];
                break;
            case "TickDown":
                clip = sounds[7];
                break;
            case "RoundStart":
                clip = sounds[8];
                break;
        }

        Debug.Log(clip.name);

        activeAudio.PlayOneShot(clip);

    }

}
