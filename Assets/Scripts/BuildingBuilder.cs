using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBuilder : MonoBehaviour {

    public enum BuildingType {
        back,
        powerPlant,
        apartment,
        barrack
    }

    private Dictionary<BuildingType, GameObject> buildingPrefabs;
    public List<BuildingType> buildingCanBuild;//暂时
    private static BuildingBuilder Instance;

    private void Awake() {
        Instance = this;
    }

    public static BuildingBuilder GetInstance() {
        return Instance;
    }

    private void Start() {
        buildingCanBuild.Add(BuildingType.powerPlant);
        buildingCanBuild.Add(BuildingType.apartment);
        buildingCanBuild.Add(BuildingType.barrack);
        buildingCanBuild.Add(BuildingType.back);

        buildingPrefabs = new Dictionary<BuildingType, GameObject>();
        buildingPrefabs.Add(BuildingType.powerPlant, Resources.Load<GameObject>("Buildings/powerPlant"));
        buildingPrefabs.Add(BuildingType.apartment, Resources.Load<GameObject>("Buildings/apartment"));
        buildingPrefabs.Add(BuildingType.barrack, Resources.Load<GameObject>("Buildings/barrack"));
    }

    public void Build(BuildingType buildingType, Land land) {
        Instantiate(buildingPrefabs[buildingType], land.transform, false);
    }
    public void Build(BuildingType buildingType) {
        Instantiate(buildingPrefabs[buildingType], LandPanel.GetInstance().GetCurPointLand().transform, false);
    }
}
