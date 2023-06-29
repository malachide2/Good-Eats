using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI; // can import after sucessful build

public class GameManager : MonoBehaviour {
    // References
    public static GameManager Instance;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private EnemyHand enemyHand;

    [SerializeField] private GameObject myPlayerGO;
    private GameObject myPlayer;

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
        myPlayer = Instantiate(myPlayerGO, Vector3.zero, Quaternion.identity);

        // Start Game
        StartCoroutine(StartGameRoutine());
    }

    #region Start Game Functions
    private IEnumerator StartGameRoutine() {
        /* // Add Each Player Data to Dictionary
        for (int playerNumber = 0; playerNumber < PhotonNetwork.PlayerList.Length; playerNumber++) {
            playerData.Add(PhotonNetwork.PlayerList[playerNumber].ActorNumber, new PlayerData("Placeholder", 0));
        } */

        deckManager.StartDecks();
        enemyHand.StartGameEnemyHand();

        yield return new WaitForSeconds(0.1f); // Delay to Let DeckManager Spawn // Unnecessary?

        // Draw Cards in Order of Player Number
        StartCoroutine(myPlayer.GetComponent<PlayerHand>().StartingDrawRoutine());

        yield return new WaitForSeconds(0.5f); // Delay to Let All Players Draw their Cards

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