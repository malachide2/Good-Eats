using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckManager : MonoBehaviour {
    // References
    private CardDatabase cardDatabase;
    [SerializeField] private GameObject[] tradePile;

    [Header("Decks")]
    public List<byte> ingredientCardDeck;
    public List<byte> recipeCardDeck;
    public List<byte> tradePileCards;

    private void Awake() {
        // References
        cardDatabase = GetComponent<CardDatabase>();
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
        // for (int i = 0; i < ingredientCardDeck.Count; i++) { Debug.Log(cardDatabase.ingredientCard[ingredientCardDeck[i]].name); }
    }

    public void StartTradePile() {
        for (int i = 0; i < 4; i++) {
            // Set Next Trade Card Active
            GameObject tradePileCard = tradePile[i];
            tradePileCard.SetActive(true);
            // Add Top Card to Trade Pile
            tradePileCard.GetComponent<IngredientCardInteractibility>().ChangeCard(ingredientCardDeck[0]);
            tradePileCards.Add(ingredientCardDeck[0]);
            // Remove Top Card
            ingredientCardDeck.RemoveAt(0);
        }
    }

    private void FillIngredientList() {
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(0); }  // Buns
        for (int i = 0; i < 5; i++) { ingredientCardDeck.Add(1); }  // Noodles
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(2); }  // Tortilla
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(3); }  // Butter
        for (int i = 0; i < 9; i++) { ingredientCardDeck.Add(4); }  // Cheese
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(5); }  // Eggs
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(6); }  // Bacon
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(7); }  // Beef
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(8); }  // Chicken
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(9); }  // Fish
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(10); } // Sausage
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(11); } // Salt
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(12); } // Corn
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(13); } // Lettuce
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(14); } // Onions
        for (int i = 0; i < 5; i++) { ingredientCardDeck.Add(15); } // Potatoes
        for (int i = 0; i < 8; i++) { ingredientCardDeck.Add(16); } // Tomatoes
    }
    private void FillRecipeList() {
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(0); }  // Bacon Egg n' Cheese
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(1); }  // BLT
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(2); }  // Burrito
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(3); }  // Cheeseburger
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(4); }  // Chicken Quesadilla
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(5); }  // Fish and Chips
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(6); }  // Fish Taco
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(7); }  // Grilled Cheese
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(8); }  // Hotdog
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(9); }  // Jambalaya
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(10); } // Omelette
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(11); } // Pizza
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(12); } // Popcorn
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(13); } // Potato Chips
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(14); } // Ramen
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(15); } // Salad
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(16); } // Spaghetti
    }
    #endregion
}