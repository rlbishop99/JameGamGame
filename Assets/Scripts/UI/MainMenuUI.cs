using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

    public GameObject instructions;
    public GameObject about;

    public GameObject musicManager;

    private void Start() {
        
        musicManager = GameObject.FindGameObjectWithTag("MusicManager");

    }

    public void PlayGame() {
        Loader.Load(Loader.Scene.ProjectileScene);
    }

    public void HowToPlay() {

        instructions.SetActive(true);

    }

    public void AboutInfo() {

        about.SetActive(true);

    }

  
    public void Quit() {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void PlaySFX(string name) {

        musicManager.GetComponent<MusicManager>().SetAndPlaySound(name);

    }
}
