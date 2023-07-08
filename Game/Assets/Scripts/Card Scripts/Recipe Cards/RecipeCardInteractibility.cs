using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RecipeCardInteractibility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public RecipeCard card;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image artworkImage;
    [SerializeField] private TextMeshProUGUI[] ingredientTexts;
    public TextMeshProUGUI pointValueText;

    [Header("References")]
    [SerializeField] private GameObject gameManagerGO;
    private CardDatabase cardDatabase;

    [SerializeField] private GameObject myPlayer;
    private PlayerUI playerUI;

    [Header("Card Popup")]
    [SerializeField] private bool isPopupCard;

    private Vector3 originalScale;

    private void Awake() {
        // References
        cardDatabase = gameManagerGO.GetComponent<CardDatabase>();

        playerUI = myPlayer.GetComponent<PlayerUI>();

        originalScale = transform.localScale;
    }

    public void RefreshCard() {
        nameText.text = card.name;
        artworkImage.sprite = card.artwork;
        pointValueText.text = card.pointValue.ToString();

        foreach (TextMeshProUGUI previousIngredientText in ingredientTexts) {
            previousIngredientText.gameObject.SetActive(false);
        }

        for (int i = 0; i < card.ingredientList.Length; i++) {
            ingredientTexts[i].gameObject.SetActive(true);
            ingredientTexts[i].text = card.ingredientList[i].name;
        }
    }

    public void ChangeCard(int cardDatabaseIndex) {
        card = cardDatabase.recipeCard[cardDatabaseIndex];
        RefreshCard();
    }

    // Called when the mouse first hovers over card
    public void OnPointerEnter(PointerEventData eventData) {
        if (isPopupCard) { return; }

        transform.position = new Vector2(transform.position.x, 225);
        transform.localScale = new Vector3(1, 1, 1);
    }

    // Called when the mouse is no longer hovering the card
    public void OnPointerExit(PointerEventData eventData) {
        if (isPopupCard) { return; }

        transform.position = new Vector2(transform.position.x, 0);
        transform.localScale = originalScale;
    }

    public void SelectCard() {
        playerUI.EnterPopup(1);
        playerUI.popupRecipeCard.GetComponent<RecipeCardInteractibility>().card = GetComponent<RecipeCardInteractibility>().card;
        playerUI.popupRecipeCard.GetComponent<RecipeCardInteractibility>().RefreshCard();
    }
}