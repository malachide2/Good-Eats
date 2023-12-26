using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;
using Unity.Collections;

public class IngredientCardInteractibility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public IngredientCard card;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image artworkImage;

    [Header("References")]
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;

    [SerializeField] private GameObject myPlayer;
    private PlayerHand playerHand;
    private PlayerController playerController;
    private PlayerUI playerUI;

    [Header("Card Popup")]
    public bool isTradePileCard;
    [SerializeField] private bool isPopupCard;

    [Header("Position")]
    [ReadOnly] public Vector2 originalPosition;
    private Vector3 targetPosition;
    private float distance;
    private bool inMotion;

    public bool lockedIn = false;

    private void Awake() {
        // References
        gameManager = gameManagerGO.GetComponent<GameManager>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        playerHand = myPlayer.GetComponent<PlayerHand>();
        playerController = myPlayer.GetComponent<PlayerController>();
        playerUI = myPlayer.GetComponent<PlayerUI>();

        originalPosition = transform.position;
    }

    void Update() {
        if (!inMotion) { return; }
        MoveCard();
    }

    private void MoveCard() {
        float animationTime = 0.5f;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, distance * GameManager.gameSpeed * Time.deltaTime / animationTime);

        // Reset Card
        if (transform.position != targetPosition) { return; }

        inMotion = false;
        transform.position = originalPosition;

        if (playerController.inTradePhase) {
            if (playerController.inDeckSwap) {
                gameObject.SetActive(false);
            }
            else {
                RefreshCard();
            }

            if (!isTradePileCard) { ResetChosen(); }
        }
        else { // Recipe Phase
            gameObject.SetActive(false);
        }
    }

    public void MoveCardFromTo(Vector2 startLocation, Vector2 endLocation) {
        transform.position = startLocation;
        targetPosition = endLocation;
        distance = (float)Math.Sqrt(Math.Pow(startLocation.x - endLocation.x, 2) + Math.Pow(startLocation.y - endLocation.y, 2));

        inMotion = true;
    }

    public void ChangeCard(IngredientCard replacementCard) {
        card = replacementCard;
        RefreshCard();
    } 

    public void RefreshCard() {
        nameText.text = card.name;
        artworkImage.sprite = card.artwork;
    }

    // Called when the mouse first hovers over card
    public void OnPointerEnter(PointerEventData eventData) {
        if (isTradePileCard || isPopupCard) { return; }
        if (lockedIn || inMotion) { return; }

        EnlargeCard();
    }

    // Called when the mouse is no longer hovering the card
    public void OnPointerExit(PointerEventData eventData) {
        if (isTradePileCard || isPopupCard) { return; }
        if (lockedIn || inMotion) { return; }

        NormalizeCard();
    }

    public void EnlargeCard() {
        transform.position = new Vector2(transform.position.x, -2);
        transform.localScale = new Vector2(1.25f, 1.25f);
    }

    public void NormalizeCard() {
        transform.position = new Vector2(transform.position.x, -2.75f);
        transform.localScale = new Vector2(1, 1);
    }

    public void SelectCard() {
        if (playerController.inTradePhase) {
            if(isTradePileCard && playerHand.swapCards.Count == 0) { return; } // Can't Select Trade Pile card first

            if (!playerHand.swapCards.Contains(gameObject)) { // If this card isn't already selected
                // If two non-trade pile cards are chosen
                if (!isTradePileCard && !(playerHand.swapCards.Count == 0)) {
                    // Reset both of them
                    playerHand.swapCards[0].GetComponent<IngredientCardInteractibility>().ResetChosen();
                    ResetChosen();
                    playerHand.swapCards.Clear();
                }
                else {
                    // Select it
                    playerHand.swapCards.Add(gameObject);
                    lockedIn = true;

                    StartCoroutine(playerHand.SwapCardsRoutine());
                }
            }
            else {
                // Unselect it
                playerHand.swapCards.Remove(gameObject);
                ResetChosen();
            }
        }
        else {
            playerUI.EnterPopup(0);
            playerUI.popupCard.GetComponent<IngredientCardInteractibility>().card = GetComponent<IngredientCardInteractibility>().card;
            playerUI.popupCard.GetComponent<IngredientCardInteractibility>().RefreshCard();
        }
    }

    public void DeselectCard() {
        playerUI.ExitPopup();
    }

    public void ResetChosen() {
        NormalizeCard();

        lockedIn = false;
    }
}