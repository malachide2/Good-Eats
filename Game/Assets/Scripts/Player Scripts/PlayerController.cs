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

    public void EndTurn() {
        isTurn = false;
        gameManager.StartNextTurn();
    }
}