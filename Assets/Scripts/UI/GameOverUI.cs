using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void Replay() {

        Debug.Log("Replaying!");
        Loader.Load(Loader.Scene.ProjectileScene);

    }

    public void ToMainMenu() {

        Loader.Load(Loader.Scene.MainMenu);

    }

    public void Quit() {

        Application.Quit();

    }
}
