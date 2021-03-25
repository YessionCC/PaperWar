using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour {

    private Dictionary<Player, List<Soldier>> playerSoldiers;

    private void Awake() {
        playerSoldiers = new Dictionary<Player, List<Soldier>>();
    }

    private void Start() {
        
    }

    public void AddSoldier(Soldier soldier) {
        Player player = soldier.GetPlayer();
        if (!playerSoldiers.ContainsKey(player)) playerSoldiers.Add(player, new List<Soldier>());
        playerSoldiers[player].Add(soldier);
    }
}
