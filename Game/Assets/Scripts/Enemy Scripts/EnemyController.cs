using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
        yield return new WaitForSeconds(1);
        
        enemyHand.DetermineCardsNeeded();

        // Ensures AI doesn't always make the best move
        int skillMove = Random.Range(0, 2);
        if (skillMove == 0) {
            MakeBestMove();
        }
        else {
            // Chooses a random trade pile card to swap with
            skillMove = Random.Range(0, 4);
            SwapWithTradePile(deckManager.tradePile[skillMove]);
        }

        enemyHand.CheckRecipeCompletion();
        enemyHand.incorrectIngredients.Clear();
        enemyHand.correctIngredients.Clear();
        gameManager.StartNextTurn();
    }

    private void MakeBestMove() {
        bool swapHasBeenDone = false;

        // Compares each card in trade pile to ingredients needed by recipe
        foreach (GameObject item in deckManager.tradePile) {
            IngredientCard tradePileCard = item.GetComponent<IngredientCardInteractibility>().card;
            // If this card is needed, swap the card in hand for this card
            if (enemyHand.ingredientsNeeded.Contains(tradePileCard)) {
                SwapWithTradePile(item);

                swapHasBeenDone = true;
                break;
            }
        }

        if (!swapHasBeenDone) {
            SwapWithDeck();
        }
    }

    private void SwapWithTradePile(GameObject cardToSwap) {
        IngredientCard card = cardToSwap.GetComponent<IngredientCardInteractibility>().card;
        enemyHand.ingredientCards[enemyHand.ingredientCards.IndexOf(enemyHand.incorrectIngredients[0])] = card;
        cardToSwap.GetComponent<IngredientCardInteractibility>().ChangeCard(enemyHand.incorrectIngredients[0]);
    }

    private void SwapWithDeck() {
        // Take any of the incorrect ingredients & Put it in deck & Draw new card
        enemyHand.ingredientCards.Remove(enemyHand.incorrectIngredients[0]);
        deckManager.ingredientCardDeck.Add(enemyHand.incorrectIngredients[0]);
        deckManager.ShuffleIngredientDeck();
        enemyHand.DrawIngredientCards(1);
    }
}