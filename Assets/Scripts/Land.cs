using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour {

    private ResourcesManager resourcesManager;
    private PipeManager pipeManager;
    private BuildingBase building;

    private Player belongPlayer;
    private Player attackPlayer;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxBelongVal;
    [SerializeField] private float belongVal;
    [SerializeField] private float health;

    public EventHandler OnHealthDamage;
    public EventHandler OnBelongDamage;
    public EventHandler OnBelongPlayerChange;

    public enum LandFunction {//在站点的上有哪些功能
        none,
        buildPipe,
        buildBuilding,
        shoot
    }

    private HashSet<LandFunction> landFunctions;

    private void Awake() {
        landFunctions = new HashSet<LandFunction>();
        AddFunction(LandFunction.buildBuilding);//默认就有建建筑功能
        health = maxHealth;
        belongVal = maxBelongVal;
    }

    private void Start() {
        pipeManager = GetComponent<PipeManager>();
        resourcesManager = GetComponent<ResourcesManager>();
    }

    public void ChangeBelong(Player player) {
        if (belongPlayer == player) return;
        belongPlayer = player;
        if (OnBelongPlayerChange != null)
            OnBelongPlayerChange.Invoke(this, EventArgs.Empty);
    }

    public void Damage(float damage, Player player) {
        if (health > 0) {
            if (player != belongPlayer)
                health -= damage;
            else health += damage;//自己人就变为恢复
            if (OnHealthDamage != null)
                OnHealthDamage.Invoke(this, EventArgs.Empty);
            health = Mathf.Clamp(health, 0, maxHealth);
            attackPlayer = null;
        }
        else if (health == 0) {
            if (belongVal > 0 && attackPlayer!=player) {
                belongVal -= damage;
                if (belongVal <= 0) {
                    belongVal = 0;
                    attackPlayer = player;
                }
            }
            else if (attackPlayer == player) {
                belongVal += damage;
                if (belongVal >= maxBelongVal) {
                    belongVal = maxBelongVal;
                    ChangeBelong(attackPlayer);
                    attackPlayer = null;
                }
            }
            if (OnBelongDamage != null)
                OnBelongDamage.Invoke(this, EventArgs.Empty);
        }
    }

    public Color GetAttackerColor() {
        if (attackPlayer == null) return belongPlayer.GetColor();
        return attackPlayer.GetColor();
    }

    public float GetHealthRate() {
        if (maxHealth == 0) return 0;
        return health / maxHealth;
    }

    public float GetBelongRate() {
        if (maxBelongVal == 0) return 0;
        return belongVal / maxBelongVal;
    }

    public void ClearHealth() {//生命清零
        health = 0;
    }

    public void FillHealth() {//生命填满
        health = maxHealth;
    }

    public PipeManager GetPipeManager() {
        return pipeManager;
    }

    public HashSet<LandFunction> GetLandFunctions(){
        return landFunctions;
    }

    public void AddFunction(LandFunction function) {
        landFunctions.Add(function);
    }

    public void RemoveFunction(LandFunction function) {
        landFunctions.Remove(function);
    }

    public void SetBuilding(BuildingBase building) {
        this.building = building;
    }

    public void RemoveBuilding() {
        if (building == null) Debug.Log("No Building Error!");
        else building.Remove();
    }

    public BuildingBase GetBuilding() {
        return building;
    }

    public Vector3 GetEdgePosition(Vector3 mousePoint) {//获得Land的边缘位置
        return (mousePoint - transform.position).normalized * 1.5f + transform.position;
    }

    public Player GetPlayer() {
        return belongPlayer;
    }

}
