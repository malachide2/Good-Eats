using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Data;
using System.Security.Cryptography;
using UnityEngine.SocialPlatforms.GameCenter;

public class MainMenu : MonoBehaviour {
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] TMP_Dropdown resolutionDropdown;

    void Awake () {
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < Screen.resolutions.Length; i++) {
            Resolution resolution = Screen.resolutions[i];
            string option = resolution.width + " x " + resolution.height;
            resolutionOptions.Add(option);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.ClearOptions();
        resolutionOptions.Reverse();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.RefreshShownValue();
    }
    
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

    public void SetQuality(int qualityIndex) {
        if (qualityIndex == 0) { QualitySettings.SetQualityLevel(5); }
        else if (qualityIndex == 1) { QualitySettings.SetQualityLevel(3); }
        else { QualitySettings.SetQualityLevel(0); }
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetGameSpeed(int gameSpeedIndex) {
        if (gameSpeedIndex == 0) { GameManager.gameSpeed = 0.5f; }
        else if (gameSpeedIndex == 1) { GameManager.gameSpeed = 1; }
        else { GameManager.gameSpeed = 2; }
    }
}
