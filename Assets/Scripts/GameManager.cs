using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager Instance;
    private List<Player> players;
    private Player curPlayer;
    [SerializeField] private List<Land> allLands;
    [SerializeField] private List<Color> colors;

    public EventHandler OnCurPlayerChange;

    public static GameManager GetInstance() {
        return Instance;
    }

    private void InitPlayers() {
        players = new List<Player>();
        players.Add(new Player(0, colors[0]));//

        players.Add(new Player(1, colors[1]));
    }

    private void InitLandBelong() {
        foreach(Land land in allLands) {
            land.ChangeBelong(players[0]);
            land.ClearHealth();
        }
        allLands[0].ChangeBelong(players[1]);
        allLands[0].FillHealth();
    }

    private void InitCastle() {
        BuildingBuilder.GetInstance().Build(BuildingBuilder.BuildingType.castle, allLands[0]);
    }

    public Player GetPlayer0() {
        return players[0];
    }

    public Player GetCurPlayer() {
        return curPlayer;
    }

    public void ChangeCurPlayer(Player player) {
        curPlayer = player;
        if (OnCurPlayerChange != null)
            OnCurPlayerChange.Invoke(this, EventArgs.Empty);
    }
    public void ChangeCurPlayer(int idx) {
        ChangeCurPlayer(players[idx]);
    }

    public void Awake() {
        Instance = this;
        InitPlayers();
        
    }

    private void Start() {
        InitLandBelong();
        InitCastle();
    }


}
