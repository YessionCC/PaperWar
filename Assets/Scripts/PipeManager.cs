using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour {

    [SerializeField] private List<Pipe> pipesFrom;//上游pipe
    [SerializeField] private List<Pipe> pipesTo;//下游pipe
    [SerializeField] private int maxForwardNum;
    private ResourcesManager resourcesManager;
    private Land land;

    private bool denyForward;//是否拒绝传递，为true则不会向上游请求

    private void Start() {
        pipesFrom = new List<Pipe>();
        resourcesManager = GetComponent<ResourcesManager>();
        land = GetComponent<Land>();
        land.AddFunction(Land.LandFunction.buildPipe);

        denyForward = true;//
        resourcesManager = GetComponent<ResourcesManager>();
        resourcesManager.OnResourcesLack += (object sender, ResourceRequire e) => {
            RequestForward(e, 0);//本站资源不足，转发请求
        };
        land.OnBelongPlayerChange+= (object sender, EventArgs e) => {
            foreach (Pipe pipe in pipesFrom) pipe.Disconnect();//被占领时断链
            foreach (Pipe pipe in pipesTo) pipe.Disconnect();
        };
    }

    private void RequestForward(ResourceRequire resourceRequire, int forwardNum) {
        if (denyForward) return;
        foreach (Pipe pipe in pipesFrom) {//如果不可以提供，向上游所有资源供给站发出请求
            pipe.ForwardRequests(resourceRequire, forwardNum+1);
        }
    }

    public void SendResources(ResourceRequire resourceRequire, Pipe toPipe, int forwardNum) {//传递请求(当前无法满足请求)或发送资源(当前满足请求)
        if (forwardNum > maxForwardNum) return;//传播距离过远(避免循环)
        if (!resourcesManager.TestUpdateResources(resourceRequire)) {
            RequestForward(resourceRequire, forwardNum + 1);
        }
        else {
            resourcesManager.UpdateResources(resourceRequire);
            toPipe.SendResources(resourceRequire);
        }
    }

    public void AddPipeFrom(Pipe pipe) {
        pipesFrom.Add(pipe);
    }

    public void RemovePipeFrom(Pipe pipe) {
        pipesFrom.Remove(pipe);
    }

    public void AddPipeTo(Pipe pipe) {
        pipesTo.Add(pipe);
    }

    public void RemovePipeTo(Pipe pipe) {
        pipesTo.Remove(pipe);
    }

    public ResourcesManager GetResourcesManager() {
        return resourcesManager;
    }

    public void SetIfDenyForward(bool ifDeny) {
        denyForward = ifDeny;
    }

}
