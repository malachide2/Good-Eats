using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientCardInteractibility : MonoBehaviour {
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

    [Header("Card Popup")]
    [SerializeField] private GameObject popupCard;
    [SerializeField] private GameObject popupRecipeCard;
    [SerializeField] private GameObject greyedOutPanel;

    [HideInInspector] public bool lockedIn = false;

    private void Awake() {
        // References
        cardDatabase = gameManagerGO.GetComponent<CardDatabase>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        playerHand = myPlayer.GetComponent<PlayerHand>();
        playerController = myPlayer.GetComponent<PlayerController>();

        popupCard = myPlayer.transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
        greyedOutPanel = myPlayer.transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
    }

    private void OnEnable() {
        RefreshCard();
    }

    public void RefreshCard() {
        nameText.text = card.name;
        artworkImage.sprite = card.artwork;
    }

    public void OnCardClicked() {
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

            playerHand.DrawIngredientCards(1);

            playerController.NextPhase();
        }
        else {
            popupCard.GetComponent<IngredientCardInteractibility>().card = GetComponent<IngredientCardInteractibility>().card;
            popupCard.SetActive(true);
            greyedOutPanel.SetActive(true);
        }
    }

    public void DeselectCard() {
        popupCard.SetActive(false);
        popupRecipeCard.SetActive(false);
        greyedOutPanel.SetActive(false);
    }

    private void ChangeCard(int cardDatabaseIndex) {
        card = cardDatabase.ingredientCard[cardDatabaseIndex];
        RefreshCard();
    }

    public void HoverOn() {
        transform.position = new Vector2(transform.position.x, 250 * transform.lossyScale.y);
        transform.localScale = new Vector2(1, 1);
    }

    public void HoverOff() {
        if (lockedIn) { return; }

        transform.position = new Vector2(transform.position.x, 0);
        transform.localScale = new Vector2(0.75f, 0.75f);
    }

    public void ResetChosen() {
        lockedIn = false;

        transform.position = new Vector2(transform.position.x, 0);
        transform.localScale = new Vector2(0.75f, 0.75f);
    }
}