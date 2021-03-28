using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LandPanel : MonoBehaviour {

    private static LandPanel Instance;
    
    private Land curPointLand;

    public EventHandler OnPointLandChange;

    private IPannelInterface[] allPannels;
    private Stack<IPannelInterface> pannelStack;

    private bool isAnyPannelShow;
    

    private void Awake() {
        Instance = this;
        isAnyPannelShow = false;
        pannelStack = new Stack<IPannelInterface>();
        allPannels = transform.GetComponentsInChildren<IPannelInterface>();
    }

    private void Start() {
        foreach(IPannelInterface pannel in allPannels) {
            pannel.Hide();
        }
    }

    public void ToShow(IPannelInterface pannel) {
        if (pannelStack.Count != 0)
            pannelStack.Peek().Hide();
        pannel.Show();
        pannelStack.Push(pannel);
        isAnyPannelShow = true;
    }

    public void ToBack() {
        if (pannelStack.Count != 0)
            pannelStack.Pop().Hide();
        if (pannelStack.Count != 0) {
            pannelStack.Peek().Show();
        }
    }

    public void HideAll() {
        if (pannelStack.Count == 0) return;
        pannelStack.Peek().Hide();
        pannelStack.Clear();
        isAnyPannelShow = false;
    }

    public static LandPanel GetInstance() {
        return Instance;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && 
            MouseStateRecorder.GetInstance().GetCurMouseState()==MouseStateRecorder.MouseState.normal) {
            //当鼠标点击站点，且此时的鼠标状态为正常
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(pos.x, pos.y), Vector2.zero);
            if (hit.collider) {
                Land cur = hit.collider.GetComponent<Land>();
                if (cur == null || cur.GetPlayer()!=GameManager.GetInstance().GetCurPlayer()) return;
                curPointLand = cur;
                if (OnPointLandChange != null) OnPointLandChange.Invoke(this, EventArgs.Empty);
                transform.position = curPointLand.transform.position;
                HideAll();
                ToShow(allPannels[0]);//展示基本面板
            }
            else {
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    HideAll();
                }
            }
        }
        
    }

    public Land GetCurPointLand() {
        return curPointLand;
    }

    public bool IfAnyPannelShow() {
        return isAnyPannelShow;
    }
}
