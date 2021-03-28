using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBuilder : MonoBehaviour {

    public enum BuildingType {
        back,
        castle,
        powerPlant,
        apartment,
        shooter
    }

    [System.Serializable]
    private class SerializeDict {
        public BuildingType buildingType;
        public PlayerDataRequire[] costs;
    }
    [SerializeField] private SerializeDict[] buildCostSer;//可视化此字典

    private Dictionary<BuildingType, GameObject> buildingPrefabs;
    private Dictionary<BuildingType, PlayerDataRequire[]> buildCost;//暂时
    public List<BuildingType> buildingCanBuild;//暂时
    private static BuildingBuilder Instance;

    private void Awake() {
        Instance = this;
        buildingPrefabs = new Dictionary<BuildingType, GameObject>();
        buildingPrefabs.Add(BuildingType.powerPlant, Resources.Load<GameObject>("Buildings/powerPlant"));
        buildingPrefabs.Add(BuildingType.apartment, Resources.Load<GameObject>("Buildings/apartment"));
        buildingPrefabs.Add(BuildingType.castle, Resources.Load<GameObject>("Buildings/castle"));
        buildingPrefabs.Add(BuildingType.shooter, Resources.Load<GameObject>("Buildings/shooter"));

        buildCost = new Dictionary<BuildingType, PlayerDataRequire[]>();
        foreach(SerializeDict dict in buildCostSer) {
            buildCost.Add(dict.buildingType, dict.costs);
        }
    }

    public static BuildingBuilder GetInstance() {
        return Instance;
    }

    private void Start() {
        buildingCanBuild.Add(BuildingType.powerPlant);
        buildingCanBuild.Add(BuildingType.apartment);
        buildingCanBuild.Add(BuildingType.shooter);
        buildingCanBuild.Add(BuildingType.back);
    }

    public void Build(BuildingType buildingType, Land land) {
        if (buildCost.ContainsKey(buildingType)) {
            bool isOK = true;
            PlayerData playerData = land.GetPlayer().GetData();
            foreach (PlayerDataRequire dataRequire in buildCost[buildingType]) {
                isOK &= playerData.UpdateData(dataRequire, false);
            }
            if (!isOK) return;//可加提示
            foreach (PlayerDataRequire dataRequire in buildCost[buildingType]) {
                playerData.UpdateData(dataRequire, true);
            }
        }
        Instantiate(buildingPrefabs[buildingType], land.transform, false);
    }
    public void Build(BuildingType buildingType) {
        Land land = LandPanel.GetInstance().GetCurPointLand();
        Build(buildingType, land);
    }
}
