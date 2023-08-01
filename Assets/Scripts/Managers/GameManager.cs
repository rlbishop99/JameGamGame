using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public GameObject GameOverUI;

    private enum State {
        WaitingToStart,
        Starting,
        Playing,
        GameOver,
    }

    private State state;

    private enum TimeEvents {
        Slowdown, 
        // Swarm,
        // GearChaos,
        // FatherTime,
    }

    private float startingTimer = 3f;
    private float eventTimerMax = 10f;
    private float eventTimer;
    private float playingTimer;
    private bool isPaused = false;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("More than one GameManager instance.");
        }
        Instance = this;

        state = State.WaitingToStart;
        Time.timeScale = 0f;
        GameInput.Instance.DisableMovement();
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        eventTimer = eventTimerMax;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (state == State.WaitingToStart) {
            state = State.Starting;
            Time.timeScale = 1f;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        TogglePause();
    }

    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                break;
            case State.Starting:
                startingTimer -= Time.deltaTime;
                if (startingTimer <= 0) {
                    state = State.Playing;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    GameInput.Instance.EnableMovement();
                }
                break;
            case State.Playing:
                playingTimer += Time.deltaTime;
                eventTimer -= Time.deltaTime;
                if (eventTimer <= 0) {
                    TriggerEvent(RandomTimeEvent());
                    eventTimer = eventTimerMax;
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private TimeEvents RandomTimeEvent() {
        return (TimeEvents)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(TimeEvents)).Length);
    }
    private void TriggerEvent(TimeEvents timeEvent) {
        switch(timeEvent) {
            case TimeEvents.Slowdown:
                PlayerController.Instance.SlowDown();
                break;
            default:
                break;
        }
    }

    public bool IsPlaying() {
        return state == State.Playing;
    }

    public bool IsStarting() {
        return state == State.Starting;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public float GetStartingTimer() {
        return startingTimer;
    }

    public float GetPlayingTimer() {
        return playingTimer;
    }

    public float GetEventTimer() {
        return eventTimer;
    }

    public float GetEventTimerMax() {
        return eventTimerMax;
    }

    public void TogglePause() {
        isPaused = !isPaused;
        if (isPaused) {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
