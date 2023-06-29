using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

public class RecipeCardDisplay : MonoBehaviour {
    public RecipeCard card;

    /* public TextMeshProUGUI nameText;
    public Image artworkImage;
    [SerializeField] private TextMeshProUGUI[] ingredientTexts;
    public TextMeshProUGUI pointValueText; */

    private void OnEnable() {
        RefreshCard();
    }

    public void RefreshCard() {
        /* nameText.text = card.name;
        artworkImage.sprite = card.artwork;
        pointValueText.text = card.pointValue.ToString();

        foreach (TextMeshProUGUI previousIngredientText in ingredientTexts) {
            previousIngredientText.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < card.ingredientList.Length; i++) {
            ingredientTexts[i].gameObject.SetActive(true);
            ingredientTexts[i].text = card.ingredientList[i].name;
        } */
    }
}