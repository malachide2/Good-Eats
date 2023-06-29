using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RecipeCardInteractibility : MonoBehaviour {
    [SerializeField] private GameObject popupCard;
    [SerializeField] private GameObject greyedOutPanel;

    public void HoverOn() {
        transform.position = new Vector2(transform.position.x, 250*transform.lossyScale.y);
        transform.localScale = new Vector2(1, 1);
    }

    public void HoverOff() {
        transform.position = new Vector2(transform.position.x, 0);
        transform.localScale = new Vector2(0.75f, 0.75f);
    }

    public void OnCardClicked() {
        popupCard.GetComponent<RecipeCardDisplay>().card = GetComponent<RecipeCardDisplay>().card;
        popupCard.SetActive(true);
        greyedOutPanel.SetActive(true);
    }
}