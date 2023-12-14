using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TopCardMotion : MonoBehaviour {
    public Vector3 position;
    public Vector3 targetPosition;

    private float speed = 3;
    [HideInInspector] public bool inMotion;

    private void Awake() {
        position = transform.position;
    }

    void Update() {
        if (!inMotion) { return; }
        MoveCard();
    }

    private void MoveCard() {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Reset Card
        if (transform.position == targetPosition) {
            inMotion = false;
            transform.position = position;
        }
    }
}
