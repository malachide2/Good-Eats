using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TopCardMotion : MonoBehaviour {
    [SerializeField] private GameManager gameManager;

    [ReadOnly] public Vector2 originalPosition;
    private Vector3 targetPosition;
    private float distance;
    private bool inMotion;

    private void Awake() {
        originalPosition = transform.position;
    }

    void Update() {
        if (!inMotion) { return; }
        MoveCard();
    }

    private void MoveCard() {
        float animationTime = 0.5f;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, distance * gameManager.gameSpeed * Time.deltaTime / animationTime);

        // Reset Card
        if (transform.position == targetPosition) {
            inMotion = false;
            transform.position = originalPosition;
        }
    }

    public void MoveCardFromTo(Vector2 startLocation, Vector2 endLocation) {
        transform.position = startLocation;
        targetPosition = endLocation;
        distance = (float)Math.Sqrt(Math.Pow(startLocation.x - endLocation.x, 2) + Math.Pow(startLocation.y - endLocation.y, 2));

        inMotion = true;
    }
}
