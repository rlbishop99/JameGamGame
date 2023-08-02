using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{

    public GameObject musicManager;

    private void Start() {

        Hide();

        musicManager = GameObject.FindGameObjectWithTag("MusicManager");
    }

    public void Resume() {

        GameManager.Instance.TogglePause();

    }

    public void PlaySFX(string name) {

        musicManager.GetComponent<MusicManager>().SetAndPlaySound(name);

    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
