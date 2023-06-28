using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour {
    // References
    private GameManager gameManager;
    private DeckManager deckManager;

    private PlayerUI playerUI;
    private PlayerHand playerHand;

    public Slider pointSlider;

    [HideInInspector] public bool isTurn;
    [HideInInspector] public int phase;
    [HideInInspector] public bool inDeckPhase;
    [HideInInspector] public bool inTradePhase;

    public int points = 0;

    private void Awake() {
        // References
        gameManager = PhotonView.Find(991).GetComponent<GameManager>();
        deckManager = PhotonView.Find(990).GetComponent<DeckManager>();

        playerUI = GetComponent<PlayerUI>();
        playerHand = GetComponent<PlayerHand>();
    }

    public void StartTurn() {
        isTurn = true;
        phase = 0;
        NextPhase();
        deckManager.PV.RequestOwnership();
        playerUI.StartTurnUI();
    }

    public void NextPhase() {
        phase++;

        if (phase == 1) {
            // Start Trade/Deck Phase
        }
        else if (phase == 2) {
            playerUI.UndoButton.SetActive(false);
            StartRecipePhase();
        }
        else if (phase == 3) {
            EndTurn();
        }
    }

    private void EndTurn() {
        if (!(PhotonNetwork.PlayerList.Length == 1)) {
            isTurn = false;
            gameManager.StartNextTurn(PhotonNetwork.LocalPlayer.ActorNumber);
            playerUI.turnPhasePanel.SetActive(false);
        }
        else {
            gameManager.StartNextTurn(0);
        }
    }

    private void StartRecipePhase() {
        inDeckPhase = false;
        inTradePhase = false;

        playerHand.CheckRecipeCompletion();
    }
}