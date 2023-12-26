using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        inTradePhase = true;
        playerUI.turnPointer.transform.localPosition = new Vector2 (-860, -318);
        playerUI.turnPointer.transform.localRotation = new Quaternion(0, 0, 180, 0);
        playerUI.turnPointer.transform.localScale = new Vector2(0.75f, 0.75f);
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