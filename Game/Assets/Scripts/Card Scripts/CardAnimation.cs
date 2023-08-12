using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardAnimation : MonoBehaviour {
    public Vector3 position;
    public Vector3 targetPosition;
    public float originalPositionX;
    private IngredientCardInteractibility thisCard;

    public float speed;
    public bool inMotion;

    void Awake() {
        thisCard = GetComponent<IngredientCardInteractibility>();
    }

    void Update() {
        if(!inMotion) { return; }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (thisCard.isTradePileCard) {
            transform.localScale = new Vector3(1 + (transform.localPosition.z*0.25f), 1 + (transform.localPosition.z*0.25f), 1);
        }

        // Reset Card
        if (transform.position == targetPosition) {
            inMotion = false;
            thisCard.RefreshCard();
            if (thisCard.isTradePileCard) {
                transform.localPosition = new Vector3(originalPositionX, 0, 0);
                transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
                transform.localScale = new Vector3(0.75f, 0.75f, 1);
            }
            else {
                transform.position = new Vector2(originalPositionX, 35);
            }
        }
    }

    public void DeterminePosition() {
        if (thisCard.isTradePileCard) {
            position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        }
        else {
            position = Camera.main.ScreenToWorldPoint(transform.position);
        }
    }
}
