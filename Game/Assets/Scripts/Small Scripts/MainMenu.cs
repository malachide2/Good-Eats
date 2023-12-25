using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject mainMenu;

    public void PlayGame() {
        SceneManager.LoadScene("Main Game");
    }

    public void OpenOptions() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptions() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
