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
    [SerializeField] private bool isTradePileCard;
    [SerializeField] private bool isPopupCard;

    private Vector3 originalScale;
    [HideInInspector] public bool lockedIn = false;

    private void Awake() {
        // References
        cardDatabase = gameManagerGO.GetComponent<CardDatabase>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        playerHand = myPlayer.GetComponent<PlayerHand>();
        playerController = myPlayer.GetComponent<PlayerController>();
        playerUI = myPlayer.GetComponent<PlayerUI>();

        originalScale = transform.localScale;
    }

    public void RefreshCard() {
        nameText.text = card.name;
        artworkImage.sprite = card.artwork;
    }

    public void ChangeCard(int cardDatabaseIndex) {
        card = cardDatabase.ingredientCard[cardDatabaseIndex];
        RefreshCard();
    }

    // Called when the mouse first hovers over card
    public void OnPointerEnter(PointerEventData eventData) {
        if (isTradePileCard || isPopupCard) { return; }

        transform.position = new Vector2(transform.position.x, 225);
        transform.localScale = new Vector3(1, 1, 1);
    }

    // Called when the mouse is no longer hovering the card
    public void OnPointerExit(PointerEventData eventData) {
        if (isTradePileCard || isPopupCard) { return; }
        if (lockedIn) { return; }

        transform.position = new Vector2(transform.position.x, 0);
        transform.localScale = originalScale;
    }

    public void SelectCard() {
        if (playerController.inTradePhase) {
            if (!playerHand.swapCards.Contains(gameObject)) {
                playerHand.swapCards.Add(gameObject);
                lockedIn = true;

                playerHand.SwapCards();
            }
            else {
                playerHand.swapCards.Remove(gameObject);
                lockedIn = false;
            }
        }
        else if (playerController.inDeckPhase) {

            deckManager.ingredientCardDeck.Add(cardDatabase.FindIngredientCard(GetComponent<IngredientCardInteractibility>().card));
            gameObject.SetActive(false);

            playerHand.DrawIngredientCards();

            playerController.NextPhase();
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
        lockedIn = false;

        transform.localPosition = new Vector2(transform.position.x, 0);
        transform.localScale = originalScale;
    }
}