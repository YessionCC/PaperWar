using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour {

    [SerializeField] private List<Land> pipeLandFrom;
    private ResourcesManager resourcesManager;
    private bool denyForward;//是否拒绝传递，为true则不会向上游请求
    private BuildingBase building;
    [SerializeField] private int maxForwardNum;//最大传递数，可能还要换个地方

    private Player belongPlayer;
    public EventHandler OnBelongPlayerChange;

    public enum LandFunction {//在站点的上有哪些功能
        none,
        buildPipe,
        buildBuilding
    }

    private HashSet<LandFunction> landFunctions;

    private void Start() {
        pipeLandFrom = new List<Land>();
        landFunctions = new HashSet<LandFunction>();
        AddFunction(LandFunction.buildPipe);//默认就有建立连接功能
        AddFunction(LandFunction.buildBuilding);//默认就有建建筑功能

        denyForward = true;//
        resourcesManager = GetComponent<ResourcesManager>();
        resourcesManager.OnResourcesLack += (object sender, ResourceRequire e) => {
            RequestForward(e, 0);//本站资源不足，转发请求
        };
    }

    public void SendResources(ResourceRequire resourceRequire, Land toLand, int forwardNum) {//传递请求(当前无法满足请求)或发送资源(当前满足请求)
        if (forwardNum > maxForwardNum) return;//传播距离过远(避免循环)
        if (!resourcesManager.TestUpdateResources(resourceRequire)) {
            RequestForward(resourceRequire, forwardNum+1);
        }
        else {
            resourcesManager.UpdateResources(resourceRequire);
            StartCoroutine("SendResourcesOneByOne", new SendArgs { require = resourceRequire, toLand = toLand });
        }
    }

    public void UpdateResources(ResourceRequire resourceRequire) {//仅在收到其他站点的资源后调用
        resourcesManager.UpdateResources(resourceRequire);
    }

    private void RequestForward(ResourceRequire resourceRequire, int forwardNum) {
        if (denyForward) return;
        foreach (Land from in pipeLandFrom) {//如果不可以提供，向上游所有资源供给站发出请求
            from.SendResources(resourceRequire, this, forwardNum+1);
            Debug.DrawLine(transform.position, from.transform.position, Color.red);
        }
    }

    private class SendArgs {
        public ResourceRequire require;
        public Land toLand;
    }

    IEnumerator SendResourcesOneByOne(SendArgs args) {
        for(int i=0; i<-args.require.updateVal; i++) {//一次发一个
            Resource.GetInstance().SendResource(this, args.toLand, args.require.resource);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetIfDenyForward(bool ifDeny) {
        denyForward = ifDeny;
    }

    public void AddPipeFrom(Land land) {
        pipeLandFrom.Add(land);
    }

    public void RemovePipeFrom(Land land) {
        pipeLandFrom.Remove(land);
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

    public Vector3 GetEdgePosition(Vector3 mousePoint) {//获得Land的边缘位置
        return (mousePoint - transform.position).normalized * 1.5f + transform.position;
    }

    public void ChangeBelong(Player player) {
        if (belongPlayer == player) return;
        belongPlayer = player;
        if (OnBelongPlayerChange != null)
            OnBelongPlayerChange.Invoke(this, EventArgs.Empty);
    }

    public Player GetPlayer() {
        return belongPlayer;
    }

}
