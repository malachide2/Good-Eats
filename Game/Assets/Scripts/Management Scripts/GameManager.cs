using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI; // can import after sucessful build

public class GameManager : MonoBehaviour {
    // References
    public static GameManager Instance;
    private DeckManager deckManager;

    [SerializeField] private GameObject myPlayer;
    private PlayerController playerController;
    private PlayerHand playerHand; 
    // [SerializeField] private EnemyHand enemyHand;

    public int numberOfPlayers = 2;

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

        StartGame();
    }

    #region Start Game Functions
    private void StartGame() {
        /* // Add Each Player Data to Dictionary
        for (int playerNumber = 0; playerNumber < PhotonNetwork.PlayerList.Length; playerNumber++) {
            playerData.Add(PhotonNetwork.PlayerList[playerNumber].ActorNumber, new PlayerData("Placeholder", 0));
        } */

        deckManager.StartDecks();
        // enemyHand.StartGameEnemyHand();

        // Draw Cards in Order of Player Number
        playerHand.StartingDraw();

        deckManager.StartTradePile();

        StartNextTurn(0);
        
    }
    #endregion

    public void StartNextTurn(int currentPlayerNumber) {
        int nextPlayerNumber = (currentPlayerNumber % numberOfPlayers) + 1;
        // myPlayer.GetComponent<PlayerController>().StartTurn();
    }

    public void EndGame() {
        myPlayer.GetComponent<PlayerUI>().gameOverScreen.SetActive(true);
        myPlayer.GetComponent<PlayerUI>().restartButton.SetActive(true);
    }

    public void UpdateSliderForOthers(int playerNumber, float value) { // This goes in AI controller
        // enemyHand.enemyHands[playerNumber].transform.GetChild(1).gameObject.GetComponent<Slider>().value = value;
    }
}