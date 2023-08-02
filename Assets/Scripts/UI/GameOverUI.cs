using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{

    public GameObject musicManager;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();

        musicManager = GameObject.FindGameObjectWithTag("MusicManager");
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOver()) {
            Show();
        } else {
            Hide();
        }
    }

    public void Replay() {
        Debug.Log("Replaying!");
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.ProjectileScene);

    }

    public void ToMainMenu() {
        Debug.Log("Going to Main Menu.");
        musicManager.GetComponent<MusicManager>().Stop();
        musicManager.GetComponent<MusicManager>().SetAndPlayMusic("MainMenu");
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.MainMenu);

    }

    public void Quit() {
        Debug.Log("Quitting game");
        Time.timeScale = 1f;
        Application.Quit();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
