using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAdapt : MonoBehaviour {

    private Button button;
    private LandPanel landPanel;
    private Land.LandFunction curFunction;

    private void Awake() {
        button = GetComponent<Button>();
    }

    void Start () {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        curFunction = Land.LandFunction.none;
        landPanel = LandPanel.GetInstance();
    }
	
    public void SetFunction(Land.LandFunction landFunction) {
        if (landFunction == curFunction) return;
        curFunction = landFunction;
        button.onClick.RemoveAllListeners();//首先清除回调
        switch (curFunction) {//对于每一种功能，切换时换函数
            case Land.LandFunction.buildPipe:
                button.onClick.AddListener(BuildPipe); break;
            case Land.LandFunction.buildBuilding:
                button.onClick.AddListener(BuildBuilding); break;
            case Land.LandFunction.shoot:
                button.onClick.AddListener(BulletShoot); break;
        }
    }

    private void BuildPipe() {
        PipeBuilder.GetInstance().BuildPipe();
        landPanel.HideAll();
    }

    private void BuildBuilding() {
        LandPanel.GetInstance().ToShow(BuilderPanel.GetInstance() as IPannelInterface);
    }

    private void BulletShoot() {
        ShootLocator.GetInstance().Shoot();
        landPanel.HideAll();
    }
}
