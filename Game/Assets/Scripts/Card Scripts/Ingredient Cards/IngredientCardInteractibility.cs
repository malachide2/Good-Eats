using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredientCardInteractibility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public IngredientCard card;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image artworkImage;

    [Header("References")]
    [SerializeField] private GameObject gameManagerGO;
    private CardDatabase cardDatabase;
    private DeckManager deckManager;

    [SerializeField] private GameObject myPlayer;
    private PlayerHand playerHand;
    private PlayerController playerController;
    private PlayerUI playerUI;

    [Header("Card Popup")]
    public bool isTradePileCard;
    [SerializeField] private bool isPopupCard;

    private Vector3 originalScale;

    [Header("Position")]
    public Vector3 position;
    public Vector3 targetPosition;
    public float originalPositionX;

    [HideInInspector] public bool lockedIn = false;
    [HideInInspector] public float speed = 1000;
    [HideInInspector] public bool inMotion;

    private void Awake() {
        // References
        cardDatabase = gameManagerGO.GetComponent<CardDatabase>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        playerHand = myPlayer.GetComponent<PlayerHand>();
        playerController = myPlayer.GetComponent<PlayerController>();
        playerUI = myPlayer.GetComponent<PlayerUI>();

        originalScale = transform.localScale;

        // Setup
        if (isTradePileCard) { speed = 10; }
    }

    void Update() {
        if (!inMotion) { return; }

        MoveCard();
    }

    public void DeterminePosition() {
        if (isTradePileCard) {
            position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        }
        else {
            position = transform.position;
            position = Camera.main.ScreenToWorldPoint(transform.position);
            position.z = -5.8f;
        }
    }

    private void MoveCard() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (isTradePileCard) {
            transform.localScale = new Vector3(1 + (transform.localPosition.z * 0.3f), 1 + (transform.localPosition.z * 0.3f), 1);
        }

        // Reset Card
        if (transform.position == targetPosition) {
            inMotion = false;
            RefreshCard();
            if (isTradePileCard) {
                transform.localPosition = new Vector3(originalPositionX, 0, 0);
                // transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
                transform.localScale = new Vector3(0.75f, 0.75f, 1);
            }
            else {
                transform.position = new Vector2(originalPositionX, 35);
                playerController.RecipePhase();
            }
        }
    }


    public void RefreshCard() {
        nameText.text = card.name;
        artworkImage.sprite = card.artwork;
    }

    public void ChangeCard(IngredientCard replacementCard) {
        card = replacementCard;
        RefreshCard();
    }

    // Called when the mouse first hovers over card
    public void OnPointerEnter(PointerEventData eventData) {
        if (isTradePileCard || isPopupCard) { return; }
        if (inMotion) { return; }

        transform.position = new Vector2(transform.position.x, 150);
        transform.localScale = new Vector3(1, 1, 1);
    }

    // Called when the mouse is no longer hovering the card
    public void OnPointerExit(PointerEventData eventData) {
        if (isTradePileCard || isPopupCard) { return; }
        if (lockedIn) { return; }

        transform.position = new Vector2(transform.position.x, 35);
        transform.localScale = originalScale;
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

                    playerHand.SwapCards();
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
        transform.position = new Vector2(transform.position.x, 35);
        transform.localScale = originalScale;

        lockedIn = false;
    }
}