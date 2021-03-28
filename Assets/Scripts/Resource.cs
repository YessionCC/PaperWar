using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    [SerializeField] private Dictionary<ResourcesEnum, GameObject> resourceObj;
    [SerializeField] private Emission bulletPrefab;
    public ExpandCircle expandCircle;
    private static Resource Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        resourceObj = new Dictionary<ResourcesEnum, GameObject>();
        resourceObj.Add(ResourcesEnum.Power, Resources.Load<GameObject>("ResourceEntity/powerEntity"));
        resourceObj.Add(ResourcesEnum.Citzen, Resources.Load<GameObject>("ResourceEntity/peopleEntity"));

    }

    public static Resource GetInstance() {
        return Instance;
    }

    public void SendResource(Vector3 initPos, Vector3 toPos, Player player, ResourcesManager toMan, ResourcesEnum resourcesEnum) {//仅发送一个资源
        GameObject obj = Instantiate(resourceObj[resourcesEnum], initPos, Quaternion.identity);
        Emission entity = obj.GetComponent<Emission>();
        entity.SetToWhere(toPos);
        entity.SetPlayer(player);
        entity.SetSpeed(0.1f);//
        entity.OnArrive += (object sender, System.EventArgs e) => {
            ResourceRequire _require = new ResourceRequire();
            _require.resource = resourcesEnum;
            _require.updateVal = 1;
            toMan.UpdateResources(_require);
        };
    }

    public void EmitBullet(Vector3 initPos, Land toLand, Player player) {
        Emission entity = Instantiate(bulletPrefab, initPos, Quaternion.identity);
        entity.SetToWhere(toLand.transform.position);
        entity.SetPlayer(player);
        entity.SetSpeed(0.1f);//
        entity.OnArrive += (object sender, System.EventArgs e) => {
            if (toLand == null || toLand.GetPlayer().Equals(player)) return;
            toLand.Damage(10, player);
        };
    }
}
