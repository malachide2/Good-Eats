using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DeckManager : MonoBehaviour {
    // References
    private GameManager gameManager;
    private CardDatabase cardDatabase;

    [SerializeField] private GameObject myPlayer;
    private PlayerController playerController;
    private PlayerHand playerHand;

    public GameObject[] tradePile;
    public TopCardMotion[] topCard;

    [Header("Decks")]
    public List<IngredientCard> ingredientCardDeck = new List<IngredientCard>();
    public List<RecipeCard> recipeCardDeck = new List<RecipeCard>();

    private void Awake() {
        // References
        gameManager = GetComponent<GameManager>();
        cardDatabase = GetComponent<CardDatabase>();

        playerController = myPlayer.GetComponent<PlayerController>();
        playerHand = myPlayer.GetComponent<PlayerHand>();
    }

    public void ShuffleIngredientDeck() {
        // Fisher-Yates Shuffle
        // Remove a random card then place the new top card in its position, repeated until there are no cards
        for (int topCardIndex = 0; topCardIndex < ingredientCardDeck.Count; topCardIndex++) {
            IngredientCard topCard = ingredientCardDeck[topCardIndex];
            int randomCardIndex = Random.Range(topCardIndex, ingredientCardDeck.Count);
            ingredientCardDeck[topCardIndex] = ingredientCardDeck[randomCardIndex];
            ingredientCardDeck[randomCardIndex] = topCard;
        }
    }

    public void ShuffleRecipeDeck() {
        // Fisher-Yates Shuffle
        // Remove a random card then place the new top card in its position, repeated until there are no cards
        for (int topCardIndex = 0; topCardIndex < recipeCardDeck.Count; topCardIndex++) {
            RecipeCard topCard = recipeCardDeck[topCardIndex];
            int randomCardIndex = Random.Range(topCardIndex, recipeCardDeck.Count);
            recipeCardDeck[topCardIndex] = recipeCardDeck[randomCardIndex];
            recipeCardDeck[randomCardIndex] = topCard;
        }
    }

    #region Start Game Functions
    public void StartDecks() {
        FillIngredientList();
        FillRecipeList();

        ShuffleIngredientDeck();
        ShuffleRecipeDeck();

        // Print the Deck in Console
        // for (int i = 0; i < ingredientCardDeck.Count; i++) { Debug.Log(ingredientCardDeck[i].name); }
    }

    public IEnumerator StartTradePileRoutine() {
        for (int i = 0; i < 4; i++) {
            topCard[i].MoveCardFromTo(topCard[i].originalPosition, new Vector2(-0.9f + (0.6f * i), 0));
        }

        yield return new WaitForSeconds(0.5f / gameManager.gameSpeed);

        for (int i = 0; i < 4; i++) {
            // Set Blank Card Active & Assign Top Card
            GameObject tradePileCard = tradePile[i];
            tradePileCard.SetActive(true);
            tradePileCard.GetComponent<IngredientCardInteractibility>().ChangeCard(ingredientCardDeck[0]);
            // Remove Top Card
            ingredientCardDeck.RemoveAt(0);
        }
    }

    private void FillIngredientList() {
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[0]); }  // Buns
        for (int i = 0; i < 5; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[1]); }  // Noodles
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[2]); }  // Tortilla
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[3]); }  // Butter
        for (int i = 0; i < 9; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[4]); }  // Cheese
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[5]); }  // Eggs
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[6]); }  // Bacon
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[7]); }  // Beef
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[8]); }  // Chicken
        for (int i = 0; i < 3; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[9]); }  // Fish
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[10]); } // Sausage
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[11]); } // Salt
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[12]); } // Corn
        for (int i = 0; i < 4; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[13]); } // Lettuce
        for (int i = 0; i < 6; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[14]); } // Onions
        for (int i = 0; i < 5; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[15]); } // Potatoes
        for (int i = 0; i < 8; i++) { ingredientCardDeck.Add(cardDatabase.ingredientCard[16]); } // Tomatoes
    }
    private void FillRecipeList() {
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[0]); }  // Bacon Egg n' Cheese
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[1]); }  // BLT
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[2]); }  // Burrito
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[3]); }  // Cheeseburger
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[4]); }  // Chicken Quesadilla
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[5]); }  // Fish and Chips
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[6]); }  // Fish Taco
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[7]); }  // Grilled Cheese
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[8]); }  // Hotdog
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[9]); }  // Jambalaya
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[10]); } // Omelette
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[11]); } // Pizza
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[12]); } // Popcorn
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[13]); } // Potato Chips
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[14]); } // Ramen
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[15]); } // Salad
        for (int i = 0; i < 1; i++) { recipeCardDeck.Add(cardDatabase.recipeCard[16]); } // Spaghetti
    }
    #endregion

    public IEnumerator SwapWithDeckRoutine() {
        if (playerHand.swapCards.Count == 1) {

            playerController.inDeckSwap = true;
            // Shuffle Card into Deck & Cleanup
            IngredientCardInteractibility oldCard = playerHand.swapCards[0].GetComponent<IngredientCardInteractibility>();
            playerHand.swapCards.Clear();
            ingredientCardDeck.Add(oldCard.card);
            ShuffleIngredientDeck();

            oldCard.DeterminePosition();
            oldCard.targetPosition = new Vector2(-2, 0);
            oldCard.transform.localScale = new Vector2(0.75f, 0.75f);
            oldCard.inMotion = true;

            yield return new WaitForSeconds(0.75f / gameManager.gameSpeed);

            StartCoroutine(playerHand.DrawIngredientCardsRoutine());

            yield return new WaitForSeconds(0.5f / gameManager.gameSpeed);

            playerController.inDeckSwap = false;
            playerController.RecipePhase();
        }
    }
}