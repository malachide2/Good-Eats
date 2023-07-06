using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHand : MonoBehaviour {
    // References
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;
    private CardDatabase cardDatabase;

    private PlayerController playerController;

    [Header("Blank Cards")]
    [SerializeField] private GameObject[] ingredientCards;
    [SerializeField] private GameObject recipeCard;

    [Header("Cards in Hand")]
    public List<GameObject> swapCards;

    private void Awake() {
        // References
        gameManager = gameManagerGO.GetComponent<GameManager>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();
        cardDatabase = gameManagerGO.GetComponent<CardDatabase>();

        playerController = GetComponent<PlayerController>();
    }

    public void StartingDraw() {
        // Draw Cards
        DrawIngredientCards();
        DrawRecipeCard();
    }

    #region Draw Card Functions
    public void DrawIngredientCards() {
        foreach (GameObject blankCard in ingredientCards) {
            if (blankCard.activeInHierarchy) { continue; }
            // Set Blank Card Active & Assign Top Card
            blankCard.SetActive(true);
            blankCard.GetComponent<IngredientCardInteractibility>().ChangeCard(deckManager.ingredientCardDeck[0]);
            // Remove Top Card
            deckManager.ingredientCardDeck.RemoveAt(0);
        }
    }

    public void DrawRecipeCard() {
        // Choose Top Card of Deck & Remove It from Deck
        RecipeCard cardDrawn = cardDatabase.recipeCard[deckManager.recipeCardDeck[0]];
        deckManager.recipeCardDeck.RemoveAt(0);

        // Set Blank Card Active & Assign the Card
        recipeCard.GetComponent<RecipeCardInteractibility>().card = cardDrawn;
        recipeCard.SetActive(true);
    }
    #endregion

    public void SwapCards() {
        if (!(swapCards.Count == 2)) { return; }

        if (gameManager.numberOfPlayers == 2) { // If one card is in trade pile, other is in hand // doesn't do anything

            IngredientCardInteractibility cardDisplay1 = swapCards[0].GetComponent<IngredientCardInteractibility>();
            IngredientCardInteractibility cardDisplay2 = swapCards[1].GetComponent<IngredientCardInteractibility>();

            IngredientCard card1 = cardDisplay1.card;

            cardDisplay1.card = cardDisplay2.card;
            cardDisplay2.card = card1;

            cardDisplay1.RefreshCard();
            cardDisplay2.RefreshCard();

            if (gameManager.numberOfPlayers == 2) { // if card 1 is the one in the trade pile, swap it, otherwise swap card 2 // doesn't do anything
                // Change Card 1
                swapCards[1].GetComponent<IngredientCardInteractibility>().ResetChosen();
            }
            else {
                // Change Card 2
                swapCards[0].GetComponent<IngredientCardInteractibility>().ResetChosen();
            }

            swapCards.Clear();

            playerController.NextPhase();
        }
        else {
   
            swapCards[0].GetComponent<IngredientCardInteractibility>().ResetChosen();
            swapCards[1].GetComponent<IngredientCardInteractibility>().ResetChosen();

            swapCards.Clear();
        }
    }

    public void CheckRecipeCompletion() {
        List<GameObject> correctIngredients = new List<GameObject>();
        List<IngredientCard> ingredientsInRecipe = new List<IngredientCard>(recipeCard.GetComponent<RecipeCardInteractibility>().card.ingredientList);
        

        for (int i = 0; i < ingredientCards.Length; i++) {
            bool bigContinue = false;
            if (!ingredientCards[i].activeInHierarchy) { continue; } // Skip the Card

            IngredientCard thisCard = ingredientCards[i].GetComponent<IngredientCardInteractibility>().card;

            foreach (GameObject correctIngredientCard in correctIngredients) {
                if (thisCard == correctIngredientCard.GetComponent<IngredientCardInteractibility>().card) {
                    bigContinue = true; // Skip the Card
                    break;
                }
            }

            if (bigContinue) { continue; }

            if (ingredientsInRecipe.Contains(thisCard)) {
                correctIngredients.Add(ingredientCards[i]);
            }
        }

        if (correctIngredients.Count == ingredientsInRecipe.Count) {
            // Recipe is completed
            /* playerController.pointSlider.value += recipeCard.GetComponent<RecipeCardDisplay>().card.pointValue;
            if (playerController.pointSlider.value >= 100) {
                playerController.pointSlider.value = 100;
                gameManager.EndGame();
            } */

            deckManager.recipeCardDeck.Add(cardDatabase.FindRecipeCard(recipeCard.GetComponent<RecipeCardInteractibility>().card));
            recipeCard.SetActive(false);
            DrawRecipeCard();

            foreach (GameObject correctIngredient in correctIngredients) {
                deckManager.ingredientCardDeck.Add(cardDatabase.FindIngredientCard(correctIngredient.GetComponent<IngredientCardInteractibility>().card));
                correctIngredient.SetActive(false);
            }

            deckManager.ShuffleDeck(deckManager.ingredientCardDeck);
            DrawIngredientCards();
        }
    }
}