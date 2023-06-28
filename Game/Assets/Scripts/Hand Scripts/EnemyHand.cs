using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class EnemyHand : MonoBehaviour {
    public GameObject[] enemyHands;

    public void StartGameEnemyHand() {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length - 1; i++) {
            enemyHands[i].SetActive(true);
        }
    }
}