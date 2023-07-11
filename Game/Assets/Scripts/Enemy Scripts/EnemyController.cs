using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    // References
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;

    private EnemyHand enemyHand;

    public int points = 0;

    private void Awake() {
        // References
        gameManager = gameManagerGO.GetComponent<GameManager>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        enemyHand = GetComponent<EnemyHand>();
    }

    public IEnumerator TakeTurnRoutine() {
        yield return new WaitForSeconds(2);
        
        MakeBestMove();
        enemyHand.CheckRecipeCompletion();

        gameManager.StartNextTurn();
    }

    private void MakeBestMove() {
        // Optimize Choosing the correct card
        // Check the 4 Trade Pile Cards


        List<IngredientCard> ingredientsNeeded = new List<IngredientCard>(enemyHand.recipeCard.ingredientList);
        List<IngredientCard> correctIngredients = new List<IngredientCard>();
        List<IngredientCard> incorrectIngredients = new List<IngredientCard>();

        // Compares each ingredient in hand to ingredients needed by recipe
        foreach (IngredientCard currentCard in enemyHand.ingredientCards) {
            // If this ingedient is in the recipe, mark it as correct & remove the need for it
            if (ingredientsNeeded.Contains(currentCard)) {
                correctIngredients.Add(currentCard);
                ingredientsNeeded.Remove(currentCard);
            }
            else {
                incorrectIngredients.Add(currentCard);
            }
        }

        bool swapHasBeenDone = false;

        // Compares each card in trade pile to ingredients needed by recipe
        foreach (GameObject item in deckManager.tradePile) {
            IngredientCard tradePileCard = item.GetComponent<IngredientCardInteractibility>().card;
            
            if (ingredientsNeeded.Contains(tradePileCard)) {
                Debug.Log("Swap");

                enemyHand.ingredientCards[enemyHand.ingredientCards.IndexOf(incorrectIngredients[0])] = tradePileCard;
                item.GetComponent<IngredientCardInteractibility>().ChangeCard(incorrectIngredients[0]);
                
                swapHasBeenDone = true;
                break;
            }
        }

        if (!swapHasBeenDone) {
            // Deck
            Debug.Log("Deck");

            enemyHand.ingredientCards.Remove(incorrectIngredients[0]);
            deckManager.ingredientCardDeck.Add(incorrectIngredients[0]);
            deckManager.ShuffleIngredientDeck();
            enemyHand.DrawIngredientCards(1);
        }
    }
}