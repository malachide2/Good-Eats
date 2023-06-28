using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.Serialization;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;

public struct PlayerData {
    public string PlayerName;
    public int NumberOfCards;

    public PlayerData(string playerName, int numberOfCards) {
        PlayerName = playerName;
        NumberOfCards = numberOfCards;
    }
}