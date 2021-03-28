using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainDataUI : MonoBehaviour {

    [System.Serializable]
    private class DataTextSer {
        public PlayerData.PlayerDataEnum dataEnum;
        public Text text;
    }
    [SerializeField] private DataTextSer[] dataTextSers;
    
    [SerializeField] private Dictionary<PlayerData.PlayerDataEnum, Text> dataText;

    private PlayerData curData;

    private void Start() {
        dataText = new Dictionary<PlayerData.PlayerDataEnum, Text>();
        foreach(DataTextSer ser in dataTextSers) {
            dataText.Add(ser.dataEnum, ser.text);
        }
        GameManager.GetInstance().OnCurPlayerChange += (object s, EventArgs e) => {
            if (curData != null) ClearDataShow(curData);
            curData = (s as GameManager).GetCurPlayer().GetData();
            SetDataShow(curData);
        };
        GameManager.GetInstance().ChangeCurPlayer(1);//暂时
    }

    private void SetDataShow(PlayerData data) {
        data.OnPlayerDataChange += Data_OnPlayerDataChange;
        data.OnPlayerDataLack += Data_OnPlayerDataLack;
    }

    private void ClearDataShow(PlayerData data) {
        data.OnPlayerDataChange -= Data_OnPlayerDataChange;
        data.OnPlayerDataLack -= Data_OnPlayerDataLack;
    }

    private void Data_OnPlayerDataChange(object s, PlayerDataRequire require) {
        dataText[require.dataEnum].text = (s as PlayerData).GetDataByEnum(require.dataEnum).ToString();
    }

    private void Data_OnPlayerDataLack(object s, PlayerDataRequire require) {

    }

}
