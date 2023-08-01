using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene {
        SampleScene,
        MainMenu,
        ProjectileScene
    }


    public static void Load(Scene targetScene) {
        SceneManager.LoadScene(targetScene.ToString());
    }

    public static void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
}
