using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : MonoBehaviour {

    private Land land;
    private PipeManager pipeManager;

    private void Start() {
        land = transform.GetComponentInParent<Land>();
        land.SetBuilding(this);
        land.RemoveFunction(Land.LandFunction.buildBuilding);//一旦建立了建筑，就移除建立功能
        pipeManager = land.GetPipeManager();
        pipeManager.SetIfDenyForward(false);//建立建筑后，开启请求传递
    }

    public void Remove() {
        land.SetBuilding(null);
        land.AddFunction(Land.LandFunction.buildBuilding);
        pipeManager.SetIfDenyForward(true);
        Destroy(gameObject);
    }

    public Land GetLandOn() {
        return land;
    }
}