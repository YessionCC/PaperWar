using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseLandPannel : MonoBehaviour, IPannelInterface {

    private Dictionary<Land.LandFunction, Sprite> functionSprite;
    private ButtonAdapt[] buttons;
    private Image[] buttonImg;
    private RectTransform rect;

    private void Awake() {
        functionSprite = new Dictionary<Land.LandFunction, Sprite>();
        functionSprite.Add(Land.LandFunction.buildPipe, Resources.Load<Sprite>("LandUISprite/connect"));
        functionSprite.Add(Land.LandFunction.buildBuilding, Resources.Load<Sprite>("LandUISprite/build"));
        functionSprite.Add(Land.LandFunction.shoot, Resources.Load<Sprite>("LandUISprite/shoot"));
        rect = GetComponent<RectTransform>();
        buttons = transform.GetComponentsInChildren<ButtonAdapt>();
        buttonImg = new Image[buttons.Length];
        for (int i = 0; i < buttons.Length; i++) {
            buttonImg[i] = buttons[i].transform.GetChild(0).GetComponent<Image>();
        }
    }

    public void Hide() {
        foreach (ButtonAdapt button in buttons) {
            button.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
        StartCoroutine("ShowAnimation");
        int index = 0;
        foreach (Land.LandFunction function in 
            LandPanel.GetInstance().GetCurPointLand().GetLandFunctions()) {
            buttons[index].gameObject.SetActive(true);
            buttons[index].SetFunction(function);
            buttonImg[index].sprite = functionSprite[function];
            index++;
        }
    }

    IEnumerator ShowAnimation() {
        rect.localScale = Vector3.zero;
        while (true) {
            rect.localScale = Vector3.Lerp(rect.localScale, Vector3.one, 0.2f);
            if (Vector3.Distance(rect.localScale, Vector3.one) < 0.01f) break;
            yield return null;
        }
    }

}
