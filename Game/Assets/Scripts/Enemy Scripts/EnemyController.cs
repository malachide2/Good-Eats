using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    // References
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;
    private DeckManager deckManager;
    [SerializeField] private PlayerUI playerUI;

    private EnemyHand enemyHand;
    public GameObject enemyHandGO;

    public Slider pointSlider;
    public int points = 0;

    public int enemyNumber;

    private void Awake() {
        // References
        gameManager = gameManagerGO.GetComponent<GameManager>();
        deckManager = gameManagerGO.GetComponent<DeckManager>();

        enemyHand = GetComponent<EnemyHand>();
    }

    public IEnumerator TakeTurnRoutine() {
        StartTurnUI();

        enemyHand.DetermineCardsNeeded();

        // Ensures AI doesn't always make the best move
        int skillMove = Random.Range(0, 2);
        if (skillMove == 0) {
            MakeBestMove();
        }
        else {
            // Chooses a random trade pile card to swap with
            skillMove = Random.Range(0, 4);
            StartCoroutine(SwapWithTradePileRoutine(deckManager.tradePile[skillMove]));
        }

        yield return new WaitForSeconds(1.2f / GameManager.gameSpeed);

        StartCoroutine(enemyHand.CheckRecipeCompletionRoutine());
    }

    private void MakeBestMove() {
        bool swapHasBeenDone = false;

        // Compares each card in trade pile to ingredients needed by recipe
        foreach (GameObject item in deckManager.tradePile) {
            IngredientCard tradePileCard = item.GetComponent<IngredientCardInteractibility>().card;
            // If this card is needed, swap the card in hand for this card
            if (enemyHand.ingredientsNeeded.Contains(tradePileCard)) {
                StartCoroutine(SwapWithTradePileRoutine(item));

                swapHasBeenDone = true;
                break;
            }
        }

        if (!swapHasBeenDone) {
            StartCoroutine(SwapWithDeckRoutine());
        }
    }

    private IEnumerator SwapWithTradePileRoutine(GameObject cardToSwap) {
        IngredientCard card = cardToSwap.GetComponent<IngredientCardInteractibility>().card;
        enemyHand.ingredientCards[enemyHand.ingredientCards.IndexOf(enemyHand.incorrectIngredients[0])] = card;
        cardToSwap.GetComponent<IngredientCardInteractibility>().ChangeCard(enemyHand.incorrectIngredients[0]);

        cardToSwap.SetActive(false);
        enemyHand.ingredientCardsGO[5].SetActive(false);
        deckManager.topCard[0].MoveCardFromTo(cardToSwap.transform.position, new Vector2(-2 + (2.75f * enemyNumber), 1.6f));
        deckManager.topCard[1].MoveCardFromTo(new Vector2(-2 + (2.75f * enemyNumber), 1.6f), cardToSwap.transform.position);

        yield return new WaitForSeconds(0.5f / GameManager.gameSpeed);

        cardToSwap.SetActive(true);
        enemyHand.ingredientCardsGO[5].SetActive(true);
    }

    private IEnumerator SwapWithDeckRoutine() {
        // Take any of the incorrect ingredients & Put it in deck & Draw new card
        enemyHand.ingredientCards.Remove(enemyHand.incorrectIngredients[0]);
        deckManager.ingredientCardDeck.Add(enemyHand.incorrectIngredients[0]);
        deckManager.ShuffleIngredientDeck();
        enemyHand.ingredientCardsGO[5].SetActive(false);
        deckManager.topCard[0].MoveCardFromTo(new Vector2(-2 + (2.75f * enemyNumber), 1.6f), new Vector2(-2, 0));
        yield return new WaitForSeconds(0.7f / GameManager.gameSpeed);
        enemyHand.DrawIngredientCards(1);
        yield return new WaitForSeconds(0.5f / GameManager.gameSpeed);
        enemyHand.ingredientCardsGO[5].SetActive(true);
    }

    private void StartTurnUI() {
        Transform transform = playerUI.turnPointer.transform;
        playerUI.turnPointer.transform.localPosition = new Vector2(-690 + (510 * enemyNumber), 475);
        playerUI.turnPointer.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
        playerUI.turnPointer.transform.localScale = new Vector2(0.5f, 0.5f);
    }
}