using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    // Create Dictionary of All Player Data
    public Dictionary<int, PlayerData> playerData = new Dictionary<int, PlayerData>();

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

        StartNextTurn(0);
        
    }
    #endregion

    public void StartNextTurn(int currentPlayerNumber) {
        int nextPlayerNumber = (currentPlayerNumber % numberOfPlayers) + 1;
        playerController.TakeTurn();

        if (currentPlayerNumber == 1) {
            enemies[0].GetComponent<EnemyHand>().CheckRecipeCompletion();
            enemies[1].GetComponent<EnemyHand>().CheckRecipeCompletion();
            enemies[2].GetComponent<EnemyHand>().CheckRecipeCompletion();
        }
    }

    public void EndGame() {
        myPlayer.GetComponent<PlayerUI>().gameOverScreen.SetActive(true);
        myPlayer.GetComponent<PlayerUI>().restartButton.SetActive(true);
    }
}