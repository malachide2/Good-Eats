using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EnemyHand : MonoBehaviour {
    [SerializeField] private GameManager gameManager;

    public GameObject[] enemyHands;

    public void StartGameEnemyHand() {
        for (int i = 0; i < (gameManager.numberOfPlayers - 1); i++) {
            enemyHands[i].SetActive(true);
        }
    }
}