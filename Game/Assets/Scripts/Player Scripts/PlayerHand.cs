using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartCoroutine(DrawIngredientCardsRoutine());
        DrawRecipeCard();
    }

    #region Draw Card Functions
    public IEnumerator DrawIngredientCardsRoutine() {
        List<IngredientCard> currentCards = new List<IngredientCard>();
        foreach (GameObject cardGO in ingredientCards) {
            if (cardGO.activeInHierarchy) {
                currentCards.Add(cardGO.GetComponent<IngredientCardInteractibility>().card);
            }
        }
        for (int i = 0; i < ingredientCards.Length - currentCards.Count; i++) {
            deckManager.topCard[i].targetPosition = new Vector2(1.5f + (-0.9f*i),-2.75f);
            deckManager.topCard[i].inMotion = true;
        }

        yield return new WaitForSeconds(1.1f);

        for (int i = 0; i < ingredientCards.Length; i++) {
            if (!ingredientCards[i].activeInHierarchy) {
                ingredientCards[i].SetActive(true);
            }

            if (i < currentCards.Count) {
                ingredientCards[i].GetComponent<IngredientCardInteractibility>().ChangeCard(currentCards[i]);
            }
            else {
                ingredientCards[i].GetComponent<IngredientCardInteractibility>().ChangeCard(deckManager.ingredientCardDeck[0]);
                deckManager.ingredientCardDeck.RemoveAt(0);
            }           
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

    public IEnumerator SwapCardsRoutine() {
        if (swapCards.Count == 2) {
            foreach (GameObject card in swapCards) {
                IngredientCardInteractibility cardInteractibility = card.GetComponent<IngredientCardInteractibility>();
                cardInteractibility.DeterminePosition();
                cardInteractibility.inMotion = true;
            }

            IngredientCardInteractibility card1 = swapCards[0].GetComponent<IngredientCardInteractibility>();
            IngredientCardInteractibility card2 = swapCards[1].GetComponent<IngredientCardInteractibility>();
            swapCards.Clear();

            IngredientCard originalCard1 = card1.card;
            card1.card = card2.card;
            card2.card = originalCard1;
            card1.targetPosition = card2.position;
            card2.targetPosition = card1.position;

            yield return new WaitForSeconds(2);

            playerController.RecipePhase();
        }
    }

    public IEnumerator CheckRecipeCompletionRoutine() {
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
                correctIngredient.GetComponent<IngredientCardInteractibility>().DeterminePosition();
                correctIngredient.GetComponent<IngredientCardInteractibility>().targetPosition = new Vector2(-2, 0);
                correctIngredient.GetComponent<IngredientCardInteractibility>().inMotion = true;
            }
            deckManager.ShuffleIngredientDeck();

            yield return new WaitForSeconds(2);

            StartCoroutine(DrawIngredientCardsRoutine());

            yield return new WaitForSeconds(2);
        }

        playerController.EndTurn();
    }
}