using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class IngredientCardInteractibility : MonoBehaviour {
    // References
    private CardDatabase cardDatabase;
    private DeckManager deckManager;

    private PlayerHand playerHand;
    private PlayerController playerController;

    [Header("Card Popup")]
    [SerializeField] private GameObject popupCard;
    [SerializeField] private GameObject popupRecipeCard;
    [SerializeField] private GameObject greyedOutPanel;

    [HideInInspector] public bool lockedIn = false;

    private void Awake() {
        // References
        cardDatabase = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<CardDatabase>();
        deckManager = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHand = player.GetComponent<PlayerHand>();
        playerController = player.GetComponent<PlayerController>();

        if (!TryGetComponent<PhotonView>(out PhotonView PV)) { return; }
        popupCard = player.transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
        greyedOutPanel = player.transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
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
            if (TryGetComponent<PhotonView>(out PhotonView PV)) { return; }

            deckManager.ingredientCardDeck.Add(cardDatabase.FindIngredientCard(GetComponent<IngredientCardDisplay>().card));
            gameObject.SetActive(false);

            playerHand.DrawIngredientCards(1);

            playerController.NextPhase();
        }
        else {
            popupCard.GetComponent<IngredientCardDisplay>().card = GetComponent<IngredientCardDisplay>().card;
            popupCard.SetActive(true);
            greyedOutPanel.SetActive(true);
        }
    }

    public void DeselectCard() {
        popupCard.SetActive(false);
        popupRecipeCard.SetActive(false);
        greyedOutPanel.SetActive(false);
    }

    [PunRPC]
    private void RPC_ChangeNetworkCard(int cardDatabaseIndex, int viewID) {
        GameObject networkCard = PhotonView.Find(viewID).gameObject;
        networkCard.GetComponent<IngredientCardDisplay>().card = cardDatabase.ingredientCard[cardDatabaseIndex];
        networkCard.GetComponent<IngredientCardDisplay>().RefreshCard();
    }

    public void HoverOn() {
        if (TryGetComponent<PhotonView>(out PhotonView PV)) { return; }

        transform.position = new Vector2(transform.position.x, 250 * transform.lossyScale.y);
        transform.localScale = new Vector2(1, 1);
    }

    public void HoverOff() {
        if (TryGetComponent<PhotonView>(out PhotonView PV)) { return; }
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