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
    public GameObject[] ingredientCardsGO;
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

            deckManager.topCard[i].MoveCardFromTo(deckManager.topCard[i].originalPosition, new Vector2(-2 + (2.75f * enemyController.enemyNumber) + (-0.35f * i), 1.6f));
        }
    }

    public void DrawRecipeCard() {
        // Assign Top Card & Remove from deck
        recipeCard = deckManager.recipeCardDeck[0];
        deckManager.recipeCardDeck.RemoveAt(0);
    }

    public IEnumerator CheckRecipeCompletionRoutine() {
        DetermineCardsNeeded();
        if (correctIngredients.Count == recipeCard.ingredientList.Length) { // If recipe is completed
            // Add points
            enemyController.points += recipeCard.pointValue;
            enemyController.pointSlider.value += recipeCard.pointValue;
            if (enemyController.pointSlider.value >= 100) {
                enemyController.pointSlider.value = 100;
                gameManager.EndGame();

                yield break;
            }

            // Get a new recipe
            deckManager.recipeCardDeck.Add(recipeCard);
            DrawRecipeCard();

            // Shuffle old ingredients into deck
            foreach (IngredientCard correctIngredientCard in correctIngredients) {
                deckManager.ingredientCardDeck.Add(correctIngredientCard);
                ingredientCards.Remove(correctIngredientCard);
            }

            for (int i = 0; i < correctIngredients.Count; i++) {
                deckManager.topCard[i].MoveCardFromTo(new Vector2(-2 + (2.75f * enemyController.enemyNumber) + (-0.35f * i), 1.6f), deckManager.topCard[i].originalPosition);
                ingredientCardsGO[5 - i].SetActive(false);
            }

            yield return new WaitForSeconds(1 / GameManager.gameSpeed);
            // Draw new ingredients
            deckManager.ShuffleIngredientDeck();
            DrawIngredientCards(correctIngredients.Count);

            yield return new WaitForSeconds(0.5f / GameManager.gameSpeed);
            for (int i = 0; i < correctIngredients.Count; i++) {
                ingredientCardsGO[5 - i].SetActive(true);
            }
        }

        yield return new WaitForSeconds(0.5f / GameManager.gameSpeed);
        gameManager.StartNextTurn();
    }

    public void DetermineCardsNeeded() {
        ingredientsNeeded = new List<IngredientCard>(recipeCard.ingredientList);
        correctIngredients.Clear();
        incorrectIngredients.Clear();

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