using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHand : MonoBehaviour {
    [SerializeField] private GameObject playerHand;

    // References
    [SerializeField] private GameManager gameManager;
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
        playerController = GetComponent<PlayerController>();
    }

    #region Start Game Functions
    public IEnumerator StartingDrawRoutine() {
        yield return new WaitForSeconds(0.4f); // Delay to Let Other Players Draw One at a Time
        // Draw Cards
        DrawIngredientCards(6);
        DrawRecipeCard();
    }
    #endregion

    #region Draw Card Functions
    public void DrawIngredientCards(int amount) {
        for (int i = 0; i < amount; i++) {
            // Choose Top Card of Deck & Remove It from Deck
            IngredientCard cardDrawn = cardDatabase.ingredientCard[deckManager.ingredientCardDeck[0]];
            deckManager.ingredientCardDeck.RemoveAt(0);

            // Add Card to Hand List & Set the Next Blank Card Active & Assign the Card
            foreach (GameObject blankCard in ingredientCards) {
                if (blankCard.activeInHierarchy) { continue; }

                blankCard.GetComponent<IngredientCardDisplay>().card = cardDrawn;
                blankCard.SetActive(true);
                break;
            }
        }
    }

    public void DrawRecipeCard() {
        // Choose Top Card of Deck & Remove It from Deck
        RecipeCard cardDrawn = cardDatabase.recipeCard[deckManager.recipeCardDeck[0]];
        deckManager.recipeCardDeck.RemoveAt(0);

        // Set Blank Card Active & Assign the Card
        recipeCard.GetComponent<RecipeCardDisplay>().card = cardDrawn;
        recipeCard.SetActive(true);
    }
    #endregion

    public void SwapCards() {
        if (!(swapCards.Count == 2)) { return; }

        if (gameManager.numberOfPlayers == 2) { // If one card is in trade pile, other is in hand // doesn't do anything

            IngredientCardDisplay cardDisplay1 = swapCards[0].GetComponent<IngredientCardDisplay>();
            IngredientCardDisplay cardDisplay2 = swapCards[1].GetComponent<IngredientCardDisplay>();

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
        List<IngredientCard> ingredientsInRecipe = new List<IngredientCard>(recipeCard.GetComponent<RecipeCardDisplay>().card.ingredientList);
        

        for (int i = 0; i < ingredientCards.Length; i++) {
            bool bigContinue = false;
            if (!ingredientCards[i].activeInHierarchy) { continue; } // Skip the Card

            IngredientCard thisCard = ingredientCards[i].GetComponent<IngredientCardDisplay>().card;

            foreach (GameObject correctIngredientCard in correctIngredients) {
                if (thisCard == correctIngredientCard.GetComponent<IngredientCardDisplay>().card) {
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

            deckManager.recipeCardDeck.Add(cardDatabase.FindRecipeCard(recipeCard.GetComponent<RecipeCardDisplay>().card));
            recipeCard.SetActive(false);
            DrawRecipeCard();

            foreach (GameObject correctIngredient in correctIngredients) {
                deckManager.ingredientCardDeck.Add(cardDatabase.FindIngredientCard(correctIngredient.GetComponent<IngredientCardDisplay>().card));
                correctIngredient.SetActive(false);
            }

            deckManager.ShuffleDeck(deckManager.ingredientCardDeck);
            DrawIngredientCards(correctIngredients.Count);
        }
    }
}