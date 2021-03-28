using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResourceUpdater : MonoBehaviour {

    private ResourcesManager resourcesManager;
    [SerializeField] private ResourceRequire[] resourcesToConsume;
    [SerializeField] private ResourceRequire[] resourcesToGen;
    [SerializeField] private PlayerDataRequire[] playerDataToGen;
    [SerializeField] private float updateFreq;
    private float innerTimer;
    private bool canUpdate;

    private Player player;

    private void Start() {
        resourcesManager = transform.GetComponentInParent<ResourcesManager>();//
        player = transform.GetComponentInParent<Land>().GetPlayer();//
        innerTimer = updateFreq;
        canUpdate = true;
    }

    private void Update() {
        if (canUpdate) {
            innerTimer -= Time.deltaTime;
            if (innerTimer <= 0) {
                bool ifResourceSat = true;
                foreach (ResourceRequire updateVal in resourcesToConsume) {
                    bool ret = resourcesManager.TestUpdateResources(updateVal);//尝试
                    if (!ret) resourcesManager.UpdateResources(updateVal);//缺少则发送管路请求
                    ifResourceSat &= ret;
                }
                innerTimer = updateFreq;
                if (!ifResourceSat) return;
                foreach (ResourceRequire updateVal in resourcesToConsume) {//所有资源都满足才一次更新
                    resourcesManager.UpdateResources(updateVal);
                }
                foreach (ResourceRequire updateVal in resourcesToGen) {
                    resourcesManager.UpdateResources(updateVal);
                }
                foreach (PlayerDataRequire updateVal in playerDataToGen) {
                    player.GetData().UpdateData(updateVal);
                }
            }
        }
    }


    public void SetCanUpdate(bool isUpdate) {
        canUpdate = isUpdate;
    }

}
