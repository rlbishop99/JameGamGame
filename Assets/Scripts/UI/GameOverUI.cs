using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{

    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;
    public GameObject timerUI;

    public GameObject musicManager;
    private int scoreValue;
    private int highScoreValue = 0;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();

        musicManager = GameObject.FindGameObjectWithTag("MusicManager");
    }

    private void Update() {

        scoreValue = (timerUI.GetComponent<TimerUI>().secondsSurvived * 1000) + (PlayerController.Instance.enemiesKilled * 750);

        if(scoreValue > PlayerPrefs.GetInt("highscore")) {

                highScoreValue = scoreValue;
                PlayerPrefs.SetInt("highscore", highScoreValue);
            
            }
        
        score.text = "Score: " + scoreValue;
        highScore.text = "High Score: " + PlayerPrefs.GetInt("highscore");

    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOver()) {
            if(scoreValue > highScoreValue) {

                highScoreValue = scoreValue;
                PlayerPrefs.SetInt("highscore", highScoreValue);
            
            }

            PlayerPrefs.Save();
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
