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

    public void TakeTurn() {
        isTurn = true;
        // playerUI.StartTurnUI();
        inTradePhase = true;
    }

    public void RecipePhase() {
        inTradePhase = false;
        playerHand.CheckRecipeCompletion();
    }

    private void EndTurn() {
        isTurn = false;
        gameManager.StartNextTurn(1); // 1 is first AI since Player is player[0]
        playerUI.turnPhasePanel.SetActive(false);
    }
}