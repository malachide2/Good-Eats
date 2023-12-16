using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableDeck : MonoBehaviour {
    // References
    [SerializeField] private DeckManager deckManager;

    private void OnMouseDown() {
        StartCoroutine(deckManager.SwapWithDeckRoutine());
    }
}