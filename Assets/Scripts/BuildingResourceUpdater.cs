using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResourceUpdater : MonoBehaviour {

    private ResourcesManager resourcesManager;
    [SerializeField] private ResourceRequire[] resourcesToConsume;
    [SerializeField] private ResourceRequire[] resourcesToGen;
    [SerializeField] private float updateFreq;
    private float innerTimer;
    private bool canUpdate;

    private void Start() {
        resourcesManager = transform.GetComponentInParent<ResourcesManager>();//
        innerTimer = updateFreq;
        canUpdate = true;
    }

    private void Update() {
        if (canUpdate) {
            innerTimer -= Time.deltaTime;
            if (innerTimer <= 0) {
                bool ifResourceSat = true;
                foreach (ResourceRequire updateVal in resourcesToConsume) {
                    ifResourceSat &= resourcesManager.UpdateResources(updateVal);
                }
                innerTimer = updateFreq;
                if (!ifResourceSat) return;
                foreach (ResourceRequire updateVal in resourcesToGen) {
                    resourcesManager.UpdateResources(updateVal);
                }
            }
        }
    }


    public void SetCanUpdate(bool isUpdate) {
        canUpdate = isUpdate;
    }

}
