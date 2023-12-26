using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour {
    // References
    [SerializeField] private GameManager gameManager;

    [Header("Gameplay")]
    public Slider pointSlider;

    public GameObject gameOverScreen;
    public GameObject turnPointer;

    [SerializeField] private GameObject backgroundBlur;

    [Header("Options")] // Options Refers to Menu including both Settings & Quit Button
    [SerializeField] private GameObject enterOptionsMenuButton;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject enterInstructionsMenuButton;
    [SerializeField] private GameObject instructionsMenu;
    [SerializeField] private GameObject instructionsPage1;
    [SerializeField] private GameObject instructionsPage2;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [Header("Popup Card")]
    public GameObject popupCard;
    public GameObject popupRecipeCard;

    void Awake() {
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

    public void EnterOptionsMenu() {
        enterOptionsMenuButton.SetActive(false);
        optionsMenu.SetActive(true);
        backgroundBlur.SetActive(true);
    }

    public void ExitOptionsMenu() {
        enterOptionsMenuButton.SetActive(true);
        optionsMenu.SetActive(false);

        if (popupCard.activeSelf || popupRecipeCard.activeSelf) { return; }
        backgroundBlur.SetActive(false);
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
        if (optionsMenu.activeSelf || instructionsMenu.activeSelf || gameOverScreen.activeSelf) { return; }

        popupCard.SetActive(false);
        popupRecipeCard.SetActive(false);
        backgroundBlur.SetActive(false);
    }

    public void EnterInstructions() {
        enterInstructionsMenuButton.SetActive(false);
        instructionsMenu.SetActive(true);
        backgroundBlur.SetActive(true);
    }

    public void ExitInstructions() {
        enterInstructionsMenuButton.SetActive(true);
        instructionsMenu.SetActive(false);

        if (popupCard.activeSelf || popupRecipeCard.activeSelf) { return; }
        backgroundBlur.SetActive(false);
    }

    public void TogglePage() {
        instructionsPage1.SetActive(!instructionsPage1.activeSelf);
        instructionsPage2.SetActive(!instructionsPage1.activeSelf);
    }

    public void EndGame() {
        gameOverScreen.SetActive(true);
        GetComponent<PlayerUI>().backgroundBlur.SetActive(true);
    }
    
    public void PlayAgain() {
        SceneManager.LoadScene("Main Game");
    }

    public void QuitGame() {
        SceneManager.LoadScene("Main Menu");
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
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