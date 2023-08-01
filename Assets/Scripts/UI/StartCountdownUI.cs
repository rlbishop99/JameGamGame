using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartCountdownUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;    
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsStarting()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetStartingTimer());
        countdownText.text = countdownNumber.ToString();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
