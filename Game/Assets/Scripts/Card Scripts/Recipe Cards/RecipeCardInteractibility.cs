using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class RecipeCardInteractibility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public RecipeCard card;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image artworkImage;
    [SerializeField] private TextMeshProUGUI[] ingredientTexts;
    public TextMeshProUGUI pointValueText;

    [Header("References")]
    [SerializeField] private GameObject gameManagerGO;
    private GameManager gameManager;

    [SerializeField] private GameObject myPlayer;
    private PlayerUI playerUI;

    [Header("Card Popup")]
    [SerializeField] private bool isPopupCard;

    [Header("Position")]
    public Vector2 position;
    public Vector3 targetPosition;
    public bool inMotion;

    private void Awake() {
        // References
        gameManager = gameManagerGO.GetComponent<GameManager>();
        playerUI = myPlayer.GetComponent<PlayerUI>();

        position = transform.position;
    }

    void Update() {
        if (!inMotion) { return; }
        MoveCard();
    }

    private void MoveCard() {
        float animationTime = 0.5f;
        float distance = (float)Math.Sqrt(Math.Pow(position.x - targetPosition.x, 2) + Math.Pow(position.y - targetPosition.y, 2));
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, distance * GameManager.gameSpeed * Time.deltaTime / animationTime);

        if (transform.position != targetPosition) { return; }

        inMotion = false;
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

    public void ChangeCard(RecipeCard replacementCard) {
        card = replacementCard;
        RefreshCard();
    }

    // Called when the mouse first hovers over card
    public void OnPointerEnter(PointerEventData eventData) {
        if (isPopupCard || inMotion) { return; }

        transform.position = new Vector2(transform.position.x, -2);
        transform.localScale = new Vector2(1.25f, 1.25f);
    }

    // Called when the mouse is no longer hovering the card
    public void OnPointerExit(PointerEventData eventData) {
        if (isPopupCard || inMotion) { return; }

        transform.position = new Vector2(transform.position.x, -2.75f);
        transform.localScale = new Vector2(1, 1);
    }

    public void SelectCard() {
        playerUI.EnterPopup(1);
        playerUI.popupRecipeCard.GetComponent<RecipeCardInteractibility>().card = GetComponent<RecipeCardInteractibility>().card;
        playerUI.popupRecipeCard.GetComponent<RecipeCardInteractibility>().RefreshCard();
    }
}