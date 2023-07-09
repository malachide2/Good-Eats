using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour {
    // References
    [SerializeField] private GameManager gameManager;

    private PlayerController playerController;
    private PlayerHand playerHand;

    [Header("Button References")]
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject swapPhaseButtons;
    public GameObject UndoButton;
    public GameObject gameOverScreen;
    public GameObject restartButton;
    public GameObject nextActionButton;
    public GameObject turnPhasePanel;
    public Slider pointSlider;

    [Header("Popup Card References")]
    public GameObject popupCard;
    public GameObject popupRecipeCard;
    [SerializeField] private GameObject backgroundBlur;

    // public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Awake() {
        // References
        playerController = GetComponent<PlayerController>();
        playerHand = GetComponent<PlayerHand>();

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

    #region Menu Functions
    public void OpenCloseOptionsMenu() {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
        settingsButton.SetActive(!settingsButton.activeSelf);
        ToggleOptionsMenu();
    }

    public void ToggleOptionsMenu() {
        options.SetActive(true);
        settings.SetActive(false);
    }

    public void ToggleSettingsMenu() {
        settings.SetActive(true);
        options.SetActive(false);
    }
    #endregion

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
        popupCard.SetActive(false);
        popupRecipeCard.SetActive(false);
        backgroundBlur.SetActive(false);
    }

    public void ChooseDeckPhase() {
        playerController.inDeckPhase = true;
        swapPhaseButtons.SetActive(false);
        UndoButton.SetActive(true);
    }

    public void ChooseTradePhase() {
        playerController.inTradePhase = true;
        swapPhaseButtons.SetActive(false);
        UndoButton.SetActive(true);
    }

    public void UndoPhaseChoice() {
        playerController.inDeckPhase = false;
        playerController.inTradePhase = false;
        swapPhaseButtons.SetActive(true);
        UndoButton.SetActive(false);

        /* foreach (GameObject card in playerHand.swapCards) {
            if (!card.TryGetComponent<PhotonView>(out PhotonView PV)) {
                card.GetComponent<IngredientCardInteractibility>().ResetChosen();
            }
        } */
        playerHand.swapCards.Clear();
    }

    public void StartTurnUI() {
        turnPhasePanel.SetActive(true);
        swapPhaseButtons.SetActive(true);
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