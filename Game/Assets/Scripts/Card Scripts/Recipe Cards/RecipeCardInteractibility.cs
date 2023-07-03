using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeCardInteractibility : MonoBehaviour {
    public RecipeCard card;

    public TextMeshProUGUI nameText;
    public Image artworkImage;
    [SerializeField] private TextMeshProUGUI[] ingredientTexts;
    public TextMeshProUGUI pointValueText;

    [SerializeField] private GameObject popupCard;
    [SerializeField] private GameObject greyedOutPanel;

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

    public void HoverOn() {
        transform.position = new Vector2(transform.position.x, 250*transform.lossyScale.y);
        transform.localScale = new Vector2(1, 1);
    }

    public void HoverOff() {
        transform.position = new Vector2(transform.position.x, 0);
        transform.localScale = new Vector2(0.75f, 0.75f);
    }

    public void OnCardClicked() {
        popupCard.GetComponent<RecipeCardInteractibility>().card = GetComponent<RecipeCardInteractibility>().card;
        popupCard.SetActive(true);
        greyedOutPanel.SetActive(true);
    }
}