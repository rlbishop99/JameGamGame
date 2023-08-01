using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
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
        Loader.Load(Loader.Scene.ProjectileScene);

    }

    public void ToMainMenu() {
        Loader.Load(Loader.Scene.MainMenu);

    }

    public void Quit() {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
