using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyHand : MonoBehaviour {
    // References
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;

    private EnemyController enemyController;

    public List<IngredientCard> ingredientCards = new List<IngredientCard>();
    public RecipeCard recipeCard;

    private void Awake() {
        // References
        gameManager = gameManagerGO.GetComponent<GameManager>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        enemyController = GetComponent<EnemyController>();
    }

    public void StartingDraw() {
        // Draw Cards
        DrawIngredientCards(6);
        DrawRecipeCard();
    }

    public void DrawIngredientCards(int numberOfCardsToDraw) {
        for (int i = 0; i < numberOfCardsToDraw; i++) {
            // Assign Top Card & Remove from deck
            ingredientCards.Add(deckManager.ingredientCardDeck[0]);
            deckManager.ingredientCardDeck.RemoveAt(0);
        }
    }

    public void DrawRecipeCard() {
        // Assign Top Card & Remove from deck
        recipeCard = deckManager.recipeCardDeck[0];
        deckManager.recipeCardDeck.RemoveAt(0);
    }

    public void CheckRecipeCompletion() {
        List<IngredientCard> correctIngredients = new List<IngredientCard>();
        List<IngredientCard> ingredientsInRecipe = new List<IngredientCard>(recipeCard.ingredientList);

        // Compares each ingredient in hand to ingredients needed by recipe
        for (int i = 0; i < ingredientCards.Count; i++) {
            bool bigContinue = false;

            // Checks to see if this ingredient was already counted (If you have multiple of the same ingredient)
            foreach (IngredientCard correctIngredientCard in correctIngredients) {
                if (ingredientCards[i] == correctIngredientCard) {
                    bigContinue = true; // Skip the Card
                    break;
                }
            }

            if (bigContinue) { continue; }

            // If this ingedient is in the recipe, mark it as correct
            if (ingredientsInRecipe.Contains(ingredientCards[i])) {
                correctIngredients.Add(ingredientCards[i]);
            }
        }

        if (correctIngredients.Count == ingredientsInRecipe.Count) { // If recipe is completed
            // Add points
            enemyController.points += recipeCard.pointValue;
            /* playerUI.pointSlider.value += recipeCard.GetComponent<RecipeCardInteractibility>().card.pointValue;
            if (playerUI.pointSlider.value >= 100) {
                playerUI.pointSlider.value = 100;
                gameManager.EndGame();
            } */

            // Get a new recipe
            deckManager.recipeCardDeck.Add(recipeCard);
            DrawRecipeCard();

            // Shuffle old ingredients into deck
            foreach (IngredientCard correctIngredientCard in correctIngredients) {
                deckManager.ingredientCardDeck.Add(correctIngredientCard);
                ingredientCards.Remove(correctIngredientCard);
            }
            // Draw new ingredients
            deckManager.ShuffleIngredientDeck();
            DrawIngredientCards(correctIngredients.Count);
        }
    }
}