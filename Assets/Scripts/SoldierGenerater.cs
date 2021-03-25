using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGenerater : MonoBehaviour {

    private Land land;
    private Player playerBelong;
    private BuildingBase building;
    private SoldierManager soldierManager;
    private ResourcesManager resourcesManager;

    private bool ifGenSoldiers;

    [SerializeField] private float genInterval;
    [SerializeField] private float genNumOnce;
    [SerializeField] private ResourceRequire[] resourceRequires;
    private float innerTimer;

    private void Start() {
        building = GetComponent<BuildingBase>();
        land = building.GetLandOn();
        resourcesManager = land.GetComponent<ResourcesManager>();
        soldierManager = land.GetComponent<SoldierManager>();
        playerBelong = land.GetPlayer();
        land.OnBelongPlayerChange += Land_OnBelongPlayerChange;
        ifGenSoldiers = true;
        innerTimer = genInterval;
    }

    private void Update() {
        if (ifGenSoldiers) {
            innerTimer -= Time.deltaTime;
            if (innerTimer <= 0) {
                innerTimer = genInterval;
                bool ifResourceSat = true;
                foreach(ResourceRequire require in resourceRequires) {
                    ifResourceSat &= resourcesManager.UpdateResources(require);
                }
                if (!ifResourceSat) return;
                for(int i=0; i<genNumOnce; i++) {
                    soldierManager.AddSoldier(Commander.GetInstance().CreateASoldier(playerBelong, land));
                }
            }
        }
    }

    private void Land_OnBelongPlayerChange(object s, EventArgs e) {
        playerBelong = land.GetPlayer();
    }

    private void OnDestroy() {
        land.OnBelongPlayerChange -= Land_OnBelongPlayerChange;
    }
}
