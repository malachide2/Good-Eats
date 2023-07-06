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

    [SerializeField] private GameObject popupCard;
    [SerializeField] private GameObject greyedOutPanel;

    private Vector3 originalScale;

    private void Awake() {
        originalScale = transform.localScale;
    }

    private void OnEnable() {
        RefreshCard();
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

    // Called when the mouse first hovers over card
    public void OnPointerEnter(PointerEventData eventData) {
        transform.position = new Vector2(transform.position.x, 225);
        transform.localScale = new Vector3(1, 1, 1);
    }

    // Called when the mouse is no longer hovering the card
    public void OnPointerExit(PointerEventData eventData) {
        transform.position = new Vector2(transform.position.x, 0);
        transform.localScale = originalScale;
    }

    public void SelectCard() {
        popupCard.GetComponent<RecipeCardInteractibility>().card = GetComponent<RecipeCardInteractibility>().card;
        popupCard.SetActive(true);
        greyedOutPanel.SetActive(true);
    }
}