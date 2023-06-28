using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectionPortal : MonoBehaviourPunCallbacks {
    private void Awake() {
        // Connect to Photon Server using Settings Assigned in Editor
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby() {
        SceneManager.LoadScene("Room Finder");
    }
}