using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    // References
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;

    private PlayerUI playerUI;
    private PlayerHand playerHand;

    [HideInInspector] public bool isTurn;
    [HideInInspector] public bool inTradePhase;
    [HideInInspector] public bool inDeckSwap;
    [HideInInspector] public bool inRecipePhase;

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

        inRecipePhase = true;
        StartCoroutine(playerHand.CheckRecipeCompletionRoutine());
    }

    public void EndTurn() {
        inRecipePhase = false;
        isTurn = false;
        gameManager.StartNextTurn();
    }
}