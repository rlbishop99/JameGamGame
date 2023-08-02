using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour {

    public GameObject musicManager;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Show();

        musicManager = GameObject.FindGameObjectWithTag("MusicManager");
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsStarting()) {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        musicManager.GetComponent<MusicManager>().SetAndPlaySound("ButtonSelect");
        gameObject.SetActive(false);
    }
}
