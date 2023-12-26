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
        StartCoroutine(DrawRecipeCardRoutine());
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
            deckManager.topCard[i].MoveCardFromTo(deckManager.topCard[i].originalPosition, new Vector2(1.5f + (-0.9f * i), -2.75f));
        }

        yield return new WaitForSeconds(0.5f / GameManager.gameSpeed);

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

    public IEnumerator DrawRecipeCardRoutine() {
        recipeCard.GetComponent<RecipeCardInteractibility>().ChangeCard(deckManager.recipeCardDeck[0]);
        playerUI.popupRecipeCard.GetComponent<RecipeCardInteractibility>().ChangeCard(deckManager.recipeCardDeck[0]);
        deckManager.recipeCardDeck.RemoveAt(0);
        playerUI.popupRecipeCard.SetActive(true);
        recipeCard.transform.position = Vector3.zero;

        yield return new WaitForSeconds(1 / GameManager.gameSpeed);

        playerUI.popupRecipeCard.SetActive(false);
        recipeCard.SetActive(true);
        recipeCard.GetComponent<RecipeCardInteractibility>().targetPosition = new Vector2(3, -2.75f);
        recipeCard.GetComponent<RecipeCardInteractibility>().position = new Vector2(0, 0);
        recipeCard.GetComponent<RecipeCardInteractibility>().inMotion = true;
    }
    #endregion

    public IEnumerator SwapCardsRoutine() {
        if (swapCards.Count == 2) {
            IngredientCardInteractibility card1 = swapCards[0].GetComponent<IngredientCardInteractibility>();
            IngredientCardInteractibility card2 = swapCards[1].GetComponent<IngredientCardInteractibility>();
            swapCards.Clear();

            IngredientCard originalCard1 = card1.card;
            card1.card = card2.card;
            card2.card = originalCard1;
            card1.MoveCardFromTo(card1.originalPosition, card2.originalPosition);
            card2.MoveCardFromTo(card2.originalPosition, card1.originalPosition);

            yield return new WaitForSeconds(0.5f / GameManager.gameSpeed);

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

                yield break;
            }

            recipeCard.GetComponent<RecipeCardInteractibility>().targetPosition = new Vector2(0, 0);
            recipeCard.GetComponent<RecipeCardInteractibility>().position = new Vector2(3, -2.75f);
            recipeCard.GetComponent<RecipeCardInteractibility>().inMotion = true;

            // Shuffle old ingredients into deck
            foreach (GameObject correctIngredient in correctIngredients) {
                IngredientCardInteractibility correctCard = correctIngredient.GetComponent<IngredientCardInteractibility>();
                deckManager.ingredientCardDeck.Add(correctCard.card);
                correctCard.MoveCardFromTo(correctCard.originalPosition, new Vector2(-2, 0));
            }
            deckManager.ShuffleIngredientDeck();

            yield return new WaitForSeconds(0.5f /  GameManager.gameSpeed); // Time for ingredient and recipe animation

            deckManager.recipeCardDeck.Add(recipeCard.GetComponent<RecipeCardInteractibility>().card);
            recipeCard.SetActive(false);
            playerUI.popupRecipeCard.GetComponent<RecipeCardInteractibility>().ChangeCard(recipeCard.GetComponent<RecipeCardInteractibility>().card);
            playerUI.popupRecipeCard.SetActive(true);

            yield return new WaitForSeconds(1 / GameManager.gameSpeed);

            StartCoroutine(DrawRecipeCardRoutine());
            StartCoroutine(DrawIngredientCardsRoutine());
            
            yield return new WaitForSeconds(1.5f / GameManager.gameSpeed);
        }

        yield return new WaitForSeconds(0.5f / GameManager.gameSpeed);
        playerController.EndTurn();
    }
}