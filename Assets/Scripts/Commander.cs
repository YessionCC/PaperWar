using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour {

    private static Commander Instance;
    private Soldier soldierPrefab;

    private void Awake() {
        Instance = this;
        soldierPrefab = Resources.Load<Soldier>("Soldier/soldier");
    }

    public static Commander GetInstance() {
        return Instance;
    }

    public Soldier CreateASoldier(Player player, Land land) {
        Soldier soldier = Instantiate(soldierPrefab, land.transform.position, Quaternion.identity);
        soldier.SetPlayer(player);
        return soldier;
    }

}
