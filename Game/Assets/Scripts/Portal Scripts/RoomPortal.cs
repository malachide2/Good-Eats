using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RoomPortal : MonoBehaviourPunCallbacks {
    [SerializeField] private Text roomName;
    [SerializeField] private GameObject startGameButton;

    private void Awake() {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void StartGame() {
        PhotonNetwork.LoadLevel("Main Game");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        PhotonNetwork.LoadLevel("Room Finder");
    }

    public override void OnMasterClientSwitched(Player newMasterClient) {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
}