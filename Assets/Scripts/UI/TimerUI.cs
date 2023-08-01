using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image eventTimerImage;

    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;    
        Hide();
    }

    private void Update()
    {
        TimeSpan ts = TimeSpan.FromSeconds(GameManager.Instance.GetPlayingTimer());
        timerText.text = string.Format("{0:0}:{1:00}:{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);
        eventTimerImage.fillAmount = GameManager.Instance.GetEventTimer() / GameManager.Instance.GetEventTimerMax();
    }
    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsPlaying()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
