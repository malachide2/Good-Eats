using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHand : MonoBehaviour {
    // References
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;

    private PlayerController playerController;
    private PlayerUI playerUI;

    [Header("Blank Cards")]
    [SerializeField] private GameObject[] ingredientCards;
    [SerializeField] private GameObject recipeCard;

    [Header("Cards in Hand")]
    public List<GameObject> swapCards;

    private void Awake() {
        // References
        gameManager = gameManagerGO.GetComponent<GameManager>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        playerController = GetComponent<PlayerController>();
        playerUI = GetComponent<PlayerUI>();
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
        // Set Blank Card Active & Assign Top Card
        recipeCard.SetActive(true);
        recipeCard.GetComponent<RecipeCardInteractibility>().ChangeCard(deckManager.recipeCardDeck[0]);
        // Remove Top Card
        deckManager.recipeCardDeck.RemoveAt(0);
    }
    #endregion

    public void SwapCards() {
        if (!(swapCards.Count == 2)) { return; }

        IngredientCardInteractibility card1 = swapCards[0].GetComponent<IngredientCardInteractibility>();
        IngredientCardInteractibility card2 = swapCards[1].GetComponent<IngredientCardInteractibility>();
        IngredientCard originalCard1 = card1.card;

        card1.card = card2.card;
        card2.card = originalCard1;
        card1.RefreshCard();
        card2.RefreshCard();

        // If card 2 is the one in the trade pile, reset card 1
        // Otherwise, reset card 2
        if (card2.isTradePileCard) { 
            card1.ResetChosen();
        }
        else {
            card2.ResetChosen();
        }

        swapCards.Clear();

        playerController.RecipePhase();
    }

    public void CheckRecipeCompletion() {
        List<GameObject> correctIngredients = new List<GameObject>();
        List<IngredientCard> ingredientsInRecipe = new List<IngredientCard>(recipeCard.GetComponent<RecipeCardInteractibility>().card.ingredientList);
        
        // Compares each ingredient in hand to ingredients needed by recipe
        for (int i = 0; i < ingredientCards.Length; i++) {
            bool bigContinue = false;
            if (!ingredientCards[i].activeInHierarchy) { continue; } // Skip the Card

            IngredientCard thisCard = ingredientCards[i].GetComponent<IngredientCardInteractibility>().card;

            // Checks to see if this ingredient was already counted (If you have multiple of the same ingredient)
            foreach (GameObject correctIngredientCard in correctIngredients) {
                if (thisCard == correctIngredientCard.GetComponent<IngredientCardInteractibility>().card) {
                    bigContinue = true; // Skip the Card
                    break;
                }
            }

            if (bigContinue) { continue; }

            // If this ingedient is in the recipe, mark it as correct
            if (ingredientsInRecipe.Contains(thisCard)) {
                correctIngredients.Add(ingredientCards[i]);
            }
        }

        if (correctIngredients.Count == ingredientsInRecipe.Count) { // If recipe is completed
            // Add points
            playerController.points += recipeCard.GetComponent<RecipeCardInteractibility>().card.pointValue;
            playerUI.pointSlider.value += recipeCard.GetComponent<RecipeCardInteractibility>().card.pointValue;
            if (playerUI.pointSlider.value >= 100) {
                playerUI.pointSlider.value = 100;
                gameManager.EndGame();
            }

            // Get a new recipe
            deckManager.recipeCardDeck.Add(recipeCard.GetComponent<RecipeCardInteractibility>().card);
            recipeCard.SetActive(false);
            DrawRecipeCard();

            // Shuffle old ingredients into deck
            foreach (GameObject correctIngredient in correctIngredients) {
                deckManager.ingredientCardDeck.Add(correctIngredient.GetComponent<IngredientCardInteractibility>().card);
                correctIngredient.SetActive(false);
            }
            // Draw new ingredients
            deckManager.ShuffleIngredientDeck();
            DrawIngredientCards();
        }

        playerController.EndTurn();
    }
}