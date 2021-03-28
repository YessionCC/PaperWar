using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    private Land land;
    private Land autoToLand;
    private Player player;
    private ResourcesManager resourcesManager;
    [SerializeField] Transform muzzlePos;
    [SerializeField] Transform turret;
    [SerializeField] private ResourceRequire[] resourcesToConsume;
    [SerializeField] private int bulletGenNum;
    [SerializeField] private float updateFreq;
    [SerializeField] private float shootInterval;
    [SerializeField] private float minShootDis;
    [SerializeField] private float maxShootDis;
    private int bulletNum;
    private float innerTimer;
    private bool canUpdate;

    private void Start() {
        bulletNum = 0;
        land=transform.GetComponentInParent<Land>();
        player = land.GetPlayer();
        land.AddFunction(Land.LandFunction.shoot);
        resourcesManager = transform.GetComponentInParent<ResourcesManager>();//
        innerTimer = updateFreq;
        canUpdate = true;

        StartCoroutine("AutoShootCor");
    }

    private void Update() {
        if (canUpdate) {
            innerTimer -= Time.deltaTime;
            if (innerTimer <= 0) {
                bool ifResourceSat = true;
                foreach (ResourceRequire updateVal in resourcesToConsume) {
                    bool ret = resourcesManager.TestUpdateResources(updateVal);
                    if(!ret) resourcesManager.UpdateResources(updateVal);
                    ifResourceSat &= ret;
                }
                innerTimer = updateFreq;
                if (!ifResourceSat) return;
                foreach (ResourceRequire updateVal in resourcesToConsume) {
                    resourcesManager.UpdateResources(updateVal);
                }
                bulletNum += bulletGenNum;
            }
        }
    }

    public void AutoShoot(Land toLand) {
        autoToLand = toLand;
    }

    public void Shoot(Land toLand) {
        if (bulletNum <= 0 || toLand==null || toLand.GetPlayer().Equals(player)) return;
        bulletNum--;
        Resource.GetInstance().EmitBullet(muzzlePos.position, toLand, player);
    }

    public bool IfDisWithin(Vector3 pos) {
        float dis = Vector3.Distance(pos, transform.position);
        return dis < maxShootDis && dis > minShootDis;
    }

    public void SetCanUpdate(bool isUpdate) {
        canUpdate = isUpdate;
    }

    IEnumerator AutoShootCor() {
        while (true) {
            Shoot(autoToLand);
            yield return new WaitForSeconds(shootInterval);
        }
    }

    public void TurretLookAt(Vector3 pos) {
        if (turret == null) return;
        Vector2 dir = pos - turret.transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
        turret.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnDestroy() {
        land.RemoveFunction(Land.LandFunction.shoot);
    }
}
