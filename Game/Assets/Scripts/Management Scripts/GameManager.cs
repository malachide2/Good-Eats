using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks {
    // References
    public static GameManager Instance;
    [HideInInspector] public PhotonView PV;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private EnemyHand enemyHand;

    [SerializeField] private GameObject myPlayerGO;
    private GameObject myPlayer;

    // Create Dictionary of All Player Data
    public Dictionary<int, PlayerData> playerData = new Dictionary<int, PlayerData>();

    private void Awake() {
        // If we started in the main game scene, load the menu scene
        if (!PhotonNetwork.IsConnected) {
            SceneManager.LoadScene("Loading Screen");
            return;
        }

        // References
        Instance = this;
        PV = GetComponent<PhotonView>();
        myPlayer = Instantiate(myPlayerGO, Vector3.zero, Quaternion.identity);

        // Start Game
        StartCoroutine(StartGameRoutine());
    }

    #region Connection Functions
    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom() {
        SceneManager.LoadScene("Room Finder");
    }
    #endregion

    #region Start Game Functions
    private IEnumerator StartGameRoutine() {
        // Add Each Player Data to Dictionary
        for (int playerNumber = 0; playerNumber < PhotonNetwork.PlayerList.Length; playerNumber++) {
            playerData.Add(PhotonNetwork.PlayerList[playerNumber].ActorNumber, new PlayerData("Placeholder", 0));
        }

        enemyHand.StartGameEnemyHand();

        yield return new WaitForSeconds(0.1f); // Delay to Let DeckManager Spawn

        // Start the Deck on Server
        if (PhotonNetwork.IsMasterClient) { deckManager.StartDecks(); }
        // Draw Cards in Order of Player Number
        StartCoroutine(myPlayer.GetComponent<PlayerHand>().StartingDrawRoutine());

        yield return new WaitForSeconds((0.4f * PhotonNetwork.PlayerList.Length) + 0.5f); // Delay to Let All Players Draw their Cards

        // Spawn the Trade Pile via the Server & Start the Turn System via the Server
        if (PhotonNetwork.IsMasterClient) {
            deckManager.PV.RequestOwnership();
            yield return new WaitForSeconds(0.1f); // Delay to Let Ownership Transfer
            deckManager.StartTradePile();

            StartNextTurn(0);
        }
    }
    #endregion

    public void StartNextTurn(int currentPlayerNumber) {
        int nextPlayerNumber = (currentPlayerNumber % PhotonNetwork.PlayerList.Length) + 1;
        PV.RPC("RPC_StartNextTurn", PhotonNetwork.PlayerList[nextPlayerNumber - 1]);
    }
    [PunRPC]
    private void RPC_StartNextTurn() {
        myPlayer.GetComponent<PlayerController>().StartTurn();
    }

    public void EndGame() {
        PV.RPC("RPC_EndGame", RpcTarget.All);
    }
    
    [PunRPC]
    private void RPC_EndGame() {
        myPlayer.GetComponent<PlayerUI>().gameOverScreen.SetActive(true);
        if (PhotonNetwork.IsMasterClient) { myPlayer.GetComponent<PlayerUI>().restartButton.SetActive(true); }
    }

    [PunRPC]
    public void RPC_UpdateSliderForOthers(int playerNumber, float value) {
        int relativePlayerNumber = playerNumber;
        if (playerNumber > (PhotonNetwork.LocalPlayer.ActorNumber-1)) {
            relativePlayerNumber = playerNumber - 1;
        }
        enemyHand.enemyHands[relativePlayerNumber].transform.GetChild(1).gameObject.GetComponent<Slider>().value = value;
    }
}