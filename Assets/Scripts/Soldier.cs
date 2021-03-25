using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    public enum SoldierState {
        Garrison,
        Attack,
        Move
    }

    private Player playerBelong;
    private SoldierState state;
    [SerializeField] public static Soldier soldierPrefab;

    public Player GetPlayer() {
        return playerBelong;
    }

    public void SetPlayer(Player player) {
        playerBelong = player;
    }

    private void Start() {
        state = SoldierState.Garrison;
    }

    private void Update() {
        switch (state) {
            case SoldierState.Garrison:
                gameObject.SetActive(false);
                break;
        }
    }

}
