using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

    public void PlayGame() {
        Loader.Load(Loader.Scene.ProjectileScene);
    }

  
    public void Quit() {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
