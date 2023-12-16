using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TopCardMotion : MonoBehaviour {
    public Vector3 position;
    public Vector3 targetPosition;

    [HideInInspector] public bool inMotion;

    private void Awake() {
        position = transform.position;
    }

    void Update() {
        if (!inMotion) { return; }
        MoveCard();
    }

    private void MoveCard() {
        float animationTime = 1;
        float distance = (float)Math.Sqrt(Math.Pow(position.x - targetPosition.x, 2) + Math.Pow(position.y - targetPosition.y, 2));
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, distance / animationTime * Time.deltaTime);

        // Reset Card
        if (transform.position == targetPosition) {
            inMotion = false;
            transform.position = position;
        }
    }
}
