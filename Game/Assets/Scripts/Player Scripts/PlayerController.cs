using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    // References
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;

    private PlayerUI playerUI;
    private PlayerHand playerHand;

    public Slider pointSlider;

    [HideInInspector] public bool isTurn;
    [HideInInspector] public int phase;
    [HideInInspector] public bool inDeckPhase;
    [HideInInspector] public bool inTradePhase;

    public int points = 0;

    private void Awake() {
        // References
        gameManager = gameManagerGO.GetComponent<GameManager>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        playerUI = GetComponent<PlayerUI>();
        playerHand = GetComponent<PlayerHand>();
    }

    public void StartTurn() {
        isTurn = true;
        phase = 0;
        NextPhase();
        playerUI.StartTurnUI();
    }

    public void NextPhase() {
        if (phase == 0) {
            // Start Trade/Deck Phase
        }
        else if (phase == 1) {
            playerUI.UndoButton.SetActive(false);
            StartRecipePhase();
        }
        else if (phase == 2) {
            EndTurn();
        }

        phase++;
    }

    private void EndTurn() {
        isTurn = false;
        gameManager.StartNextTurn(1); // 1 is first AI since Player is player[0]
        playerUI.turnPhasePanel.SetActive(false);
    }

    private void StartRecipePhase() {
        inDeckPhase = false;
        inTradePhase = false;

        playerHand.CheckRecipeCompletion();
    }
}