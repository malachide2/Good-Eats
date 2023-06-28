using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;

public class DeckManager : MonoBehaviour, IPunObservable {
    // References
    [HideInInspector] public PhotonView PV;
    private CardDatabase cardDatabase;
    [SerializeField] private GameObject[] tradePile;

    [Header("Decks")]
    public List<byte> ingredientCardDeck;
    public List<byte> recipeCardDeck;
    public List<byte> tradePileCards;

    private void Awake() {
        if (!PhotonNetwork.IsConnected) { return; }

        // References
        PV = GetComponent<PhotonView>();
        cardDatabase = GetComponent<CardDatabase>();
    }

    [PunRPC]
    private void RPC_SpawnNetworkCard(int cardDatabaseIndex, int tradePileIndex) {
        GameObject networkCard = tradePile[tradePileIndex];
        networkCard.GetComponent<IngredientCardDisplay>().card = cardDatabase.ingredientCard[cardDatabaseIndex];
        networkCard.SetActive(true);
    }

    public void ShuffleDeck(List<byte> Deck) {
        // Fisher-Yates Shuffle
        // Remove a random card then place the new top card in its position, repeated until there are no cards
        for (int topCardIndex = 0; topCardIndex < Deck.Count; topCardIndex++) {
            byte topCard = Deck[topCardIndex];
            int randomCardIndex = Random.Range(topCardIndex, Deck.Count);
            Deck[topCardIndex] = Deck[randomCardIndex];
            Deck[randomCardIndex] = topCard;
        }
    }

    #region Start Game Functions
    public void StartDecks() {
        ingredientCardDeck = new List<byte>();
        recipeCardDeck = new List<byte>();

        FillIngredientList();
        FillRecipeList();

        ShuffleDeck(ingredientCardDeck);
        ShuffleDeck(recipeCardDeck);

        // Print the Deck in Console
        for (int i = 0; i < ingredientCardDeck.Count; i++) { Debug.Log(cardDatabase.ingredientCard[ingredientCardDeck[i]].name); }
    }

    public void StartTradePile() {
        for (int i = 0; i < 4; i++) {
            // Choose Top Card of Deck & Remove It from Deck & Add It to Pile
            int cardDrawnIndex = ingredientCardDeck[0];
            tradePileCards.Add((byte)cardDrawnIndex);
            ingredientCardDeck.RemoveAt(0);
            PV.RPC("RPC_SpawnNetworkCard", RpcTarget.All, cardDrawnIndex, i);
        }
    }

    private void FillIngredientList() {
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(0); }
        for (int i = 0; i < 5; i++) { ingredientCardDeck.Add(1); }
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(2); }
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(3); }
        for (int i = 0; i < 9; i++) { ingredientCardDeck.Add(4); }
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(5); }
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(6); }
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(7); }
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(8); }
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(9); }
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(10); }
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(11); }
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(12); }
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(13); }
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(14); }
        for (int i = 0; i < 5; i++) { ingredientCardDeck.Add(15); }
        for (int i = 0; i < 8; i++) { ingredientCardDeck.Add(16); }
    }
    private void FillRecipeList() {
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(0); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(1); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(2); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(3); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(4); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(5); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(6); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(7); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(8); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(9); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(10); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(11); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(12); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(13); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(14); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(15); }
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(16); }
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            byte[] convertedDeckIngredients = ingredientCardDeck.ToArray();
            stream.SendNext(convertedDeckIngredients);

            byte[] convertedDeckRecipes = recipeCardDeck.ToArray();
            stream.SendNext(convertedDeckRecipes);

            byte[] convertedTradePileCards = tradePileCards.ToArray();
            stream.SendNext(convertedTradePileCards);
        } else if (stream.IsReading) {
            byte[] convertedDeckIngredients = (byte[])stream.ReceiveNext();
            ingredientCardDeck = new List<byte>(convertedDeckIngredients);

            byte[] convertedDeckRecipes = (byte[])stream.ReceiveNext();
            recipeCardDeck = new List<byte>(convertedDeckRecipes);

            byte[] convertedTradePileCards = (byte[])stream.ReceiveNext();
            tradePileCards = new List<byte>(convertedTradePileCards);
        }
    }
}