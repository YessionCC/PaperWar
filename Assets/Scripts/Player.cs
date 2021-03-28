using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    private int playerID;
    private Color color;
    private PlayerData playerData;

    public Player(int ID, Color col) {
        playerData = new PlayerData();
        playerID = ID;
        color = col;
    }

    public Color GetColor() {
        return color;
    }

    public PlayerData GetData() {
        return playerData;
    }
}
