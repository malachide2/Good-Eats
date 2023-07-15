using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // References
    public static GameManager Instance;
    private DeckManager deckManager;

    [SerializeField] private GameObject myPlayer;
    private PlayerController playerController;
    private PlayerHand playerHand;

    public int numberOfPlayers = 4;
    public GameObject[] enemies;
    private int playerNumber = -1;

    private void Awake() {
        // If we started in the main game scene, load the menu scene
       /* if (not in the right scene?) {
            SceneManager.LoadScene("Loading Screen");
            return;
        } */

        // References
        Instance = this;
        deckManager = GetComponent<DeckManager>();

        playerController = myPlayer.GetComponent<PlayerController>();
        playerHand = myPlayer.GetComponent<PlayerHand>();
    }

    private void Start() {
        StartGame();
    }

    #region Start Game Functions
    private void StartGame() {
        /* // Add Each Player Data to Dictionary
        for (int playerNumber = 0; playerNumber < PhotonNetwork.PlayerList.Length; playerNumber++) {
            playerData.Add(PhotonNetwork.PlayerList[playerNumber].ActorNumber, new PlayerData("Placeholder", 0));
        } */

        deckManager.StartDecks();

        // Draw Cards in Order of Player Number
        playerHand.StartingDraw();
        foreach (GameObject enemy in enemies) {
            enemy.GetComponent<EnemyHand>().StartingDraw();
        }

        deckManager.StartTradePile();

        StartNextTurn();
        
    }
    #endregion

    public void StartNextTurn() {
        playerNumber = (playerNumber + 1) % numberOfPlayers;
        
        if (playerNumber == 0) {
            playerController.TakeTurn();
        }
        else {
            StartCoroutine(enemies[playerNumber - 1].GetComponent<EnemyController>().TakeTurnRoutine()); // - 1 because 0 is first index
        }
    }

    public void EndGame() {
        myPlayer.GetComponent<PlayerUI>().gameOverScreen.SetActive(true);
        myPlayer.GetComponent<PlayerUI>().restartButton.SetActive(true);
    }
}