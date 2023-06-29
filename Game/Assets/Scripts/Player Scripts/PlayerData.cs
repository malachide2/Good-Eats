using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct PlayerData {
    public string PlayerName;
    public int NumberOfCards;

    public PlayerData(string playerName, int numberOfCards) {
        PlayerName = playerName;
        NumberOfCards = numberOfCards;
    }
}