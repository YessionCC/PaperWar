using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    [SerializeField] private Dictionary<ResourcesEnum, GameObject> resourceObj;
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

    public void SendResource(Land from, Land to, ResourcesEnum resourcesEnum) {//仅发送一个资源
        Vector3 initPos = from.GetEdgePosition(to.transform.position);
        Vector3 toPos = to.GetEdgePosition(from.transform.position);
        GameObject obj = Instantiate(resourceObj[resourcesEnum], initPos, Quaternion.identity);
        ResourceEntity entity = obj.GetComponent<ResourceEntity>();
        entity.SetToWhere(toPos);
        entity.SetSpeed(0.1f);//
        entity.OnArrive += (object sender, System.EventArgs e) => {
            ResourceRequire _require = new ResourceRequire();
            _require.resource = resourcesEnum;
            _require.updateVal = 1;
            if (to != null) to.UpdateResources(_require);
        };
    }
}
