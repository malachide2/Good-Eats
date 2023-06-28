using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RoomFinderPortal : MonoBehaviourPunCallbacks {
    // Input Fields from the Room Finder Scene
    [SerializeField] private InputField createInput;
    [SerializeField] private InputField joinInput;

    public void CreateRoom() {
        if (string.IsNullOrEmpty(createInput.text)) { return; }

        PhotonNetwork.CreateRoom(createInput.text);
    }
    public void JoinRoom() {
        if (string.IsNullOrEmpty(joinInput.text)) { return; }

        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("Room");
    }
}
