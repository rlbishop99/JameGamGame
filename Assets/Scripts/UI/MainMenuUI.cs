using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

    public GameObject instructions;

    public void PlayGame() {
        Loader.Load(Loader.Scene.ProjectileScene);
    }

    public void HowToPlay() {

        instructions.SetActive(true);

    }

  
    public void Quit() {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
