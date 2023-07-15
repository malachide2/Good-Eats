using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour {
    // References
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;

    private EnemyController enemyController;

    public List<IngredientCard> ingredientCards = new List<IngredientCard>();
    public RecipeCard recipeCard;

    public List<IngredientCard> ingredientsNeeded;
    public List<IngredientCard> correctIngredients = new List<IngredientCard>();
    public List<IngredientCard> incorrectIngredients = new List<IngredientCard>();

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
        if (correctIngredients.Count == recipeCard.ingredientList.Length) { // If recipe is completed
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

    public void DetermineCardsNeeded() {
        ingredientsNeeded = new List<IngredientCard>(recipeCard.ingredientList);

        // Compares each ingredient in hand to ingredients needed by recipe
        foreach (IngredientCard currentCard in ingredientCards) {
            // If this ingedient is in the recipe, mark it as correct & remove the need for it
            if (ingredientsNeeded.Contains(currentCard)) {
                correctIngredients.Add(currentCard);
                ingredientsNeeded.Remove(currentCard);
            }
            else {
                incorrectIngredients.Add(currentCard);
            }
        }
    }
}