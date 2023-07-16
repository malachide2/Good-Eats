using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour {
    // References
    [SerializeField] private GameManager gameManager;

    [Header("Gameplay")]
    public Slider pointSlider;

    public GameObject gameOverScreen;
    public GameObject restartButton;

    [SerializeField] private GameObject backgroundBlur;

    [Header("Options")] // Options Refers to Menu including both Settings & Quit Button, Settings refers to Hardware settings (FPS, Volume, Etc) 
    [SerializeField] private GameObject enterOptionsMenuButton;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject optionsMenuPanel;

    [SerializeField] private GameObject settingsMenu;
    // public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    [Header("Popup Card")]
    public GameObject popupCard;
    public GameObject popupRecipeCard;

    private void Awake() {

        /* int currentResolutionIndex = 0;
        int currentRefreshRate = Screen.currentResolution.refreshRate;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].refreshRate != currentRefreshRate) { continue; }
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue(); */
    }

    public void EnterOptionsMenu() {
        enterOptionsMenuButton.SetActive(false);
        optionsMenu.SetActive(true);
        optionsMenuPanel.SetActive(true);
        backgroundBlur.SetActive(true);
    }

    public void ExitOptionsMenu() {
        enterOptionsMenuButton.SetActive(true);
        optionsMenu.SetActive(false);
        optionsMenuPanel.SetActive(false);

        if (popupCard.activeSelf || popupRecipeCard.activeSelf) { return; }
        backgroundBlur.SetActive(false);
    }

    public void ToggleSettingsMenu() {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void EnterPopup(int choice) {
        if (choice == 0) { // Ingredient Card was clicked
            popupCard.SetActive(true);
        }
        else { // Recipe Card was clicked
            popupRecipeCard.SetActive(true);
        }
        backgroundBlur.SetActive(true);
    }

    public void ExitPopup() {
        if (optionsMenuPanel.activeSelf) { return; }

        popupCard.SetActive(false);
        popupRecipeCard.SetActive(false);
        backgroundBlur.SetActive(false);
    }

    public void StartTurnUI() {
        
    }

    public void SetVolume(float volume) {

    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel((qualityIndex * 2) + 1);
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
}