using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager: MonoBehaviour {

    private Dictionary<ResourcesEnum, int> resources;
    public event EventHandler OnUpdateSuccess;
    public event EventHandler<ResourceRequire> OnResourcesLack;

    private void Awake() {
        resources = new Dictionary<ResourcesEnum, int>();
    }

    public bool TestUpdateResources(ResourceRequire resourceRequire) {//只判断不更新
        if (!resources.ContainsKey(resourceRequire.resource)) resources[resourceRequire.resource] = 0;
        return resources[resourceRequire.resource] + resourceRequire.updateVal >= 0;
    }

    public bool UpdateResources(ResourceRequire resourceRequire) {
        ResourcesEnum resource = resourceRequire.resource;
        int updateVal = resourceRequire.updateVal;
        if (!resources.ContainsKey(resource)) resources[resource] = 0;
        if (resources[resource] + updateVal < 0 && OnResourcesLack != null) {
            OnResourcesLack.Invoke(this, resourceRequire);//当缺少资源
            return false;
        }
        else {
            resources[resource] += updateVal;
            if (OnUpdateSuccess != null)
                OnUpdateSuccess.Invoke(this, EventArgs.Empty);//当更新成功资源
            return true;
        }
    }

    private float timer = 1f;
    private void Update() {
        timer -= Time.deltaTime;
        if(timer<=0) {
            foreach(KeyValuePair<ResourcesEnum, int> et in resources) {
                Debug.Log(this +": " + et.Key + " " + et.Value);
            }
            timer = 1f;
        }
    }
}
