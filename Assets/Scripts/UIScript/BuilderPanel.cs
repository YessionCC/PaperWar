using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderPanel : MonoBehaviour, IPannelInterface {

    private static BuilderPanel Instance;
    private Dictionary<BuildingBuilder.BuildingType, Sprite> buildingTypeSprite;

    private Button[] buttons;
    private Image[] buttonImg;

    private void Awake() {
        Instance = this;
        buildingTypeSprite = new Dictionary<BuildingBuilder.BuildingType, Sprite>();
        buildingTypeSprite.Add(BuildingBuilder.BuildingType.apartment, Resources.Load<Sprite>("BuildUISprite/apartment"));
        buildingTypeSprite.Add(BuildingBuilder.BuildingType.powerPlant, Resources.Load<Sprite>("BuildUISprite/powerPlant"));
        buildingTypeSprite.Add(BuildingBuilder.BuildingType.shooter, Resources.Load<Sprite>("BuildUISprite/shooter"));
        buildingTypeSprite.Add(BuildingBuilder.BuildingType.back, Resources.Load<Sprite>("LandUISprite/back"));

        buttons = transform.GetComponentsInChildren<Button>(true);
        buttonImg = new Image[buttons.Length];
        int idx = 0;
        foreach (Button button in buttons) {
            buttonImg[idx++] = button.transform.GetChild(0).GetComponent<Image>();
        }
    }

    public static BuilderPanel GetInstance() {
        return Instance;
    }

    public void Hide() {
        foreach (Button button in buttons) {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void Show() {
        int idx = 0;
        foreach(BuildingBuilder.BuildingType buildingType in BuildingBuilder.GetInstance().buildingCanBuild) {
            buttons[idx].gameObject.SetActive(true);
            buttonImg[idx].sprite = buildingTypeSprite[buildingType];
            if (buildingType == BuildingBuilder.BuildingType.back)
                buttons[idx].onClick.AddListener(() => { LandPanel.GetInstance().ToBack(); });
            else 
                buttons[idx].onClick.AddListener(() => {
                    BuildingBuilder.GetInstance().Build(buildingType);
                    LandPanel.GetInstance().HideAll();
                });
            idx++;
        }
        gameObject.SetActive(true);
    }

}
