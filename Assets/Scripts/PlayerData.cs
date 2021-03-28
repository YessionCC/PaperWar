using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

	public enum PlayerDataEnum {
        capital,
        science
    }

    private Dictionary<PlayerDataEnum, int> data;
    public EventHandler<PlayerDataRequire> OnPlayerDataChange;
    public EventHandler<PlayerDataRequire> OnPlayerDataLack;

    public PlayerData() {
        data = new Dictionary<PlayerDataEnum, int>();

    }

    public bool UpdateData(PlayerDataRequire require, bool isReallyUpdate = true) {//isReallyUpdate为false时相当于仅检测
        PlayerDataEnum dataEnum = require.dataEnum;
        int val = require.updateVal;
        if (!data.ContainsKey(dataEnum)) data.Add(dataEnum, 0);
        if (data[dataEnum] + val < 0) {
            if (OnPlayerDataLack != null)
                OnPlayerDataLack.Invoke(this, require);
            return false;
        }
        else {
            if (isReallyUpdate) {
                data[dataEnum] += val;
                if (OnPlayerDataChange != null)
                    OnPlayerDataChange.Invoke(this, require);
            }
            return true;
        }
    }

    public int GetDataByEnum(PlayerDataEnum dataEnum) {
        if (!data.ContainsKey(dataEnum)) data.Add(dataEnum, 0);
        return data[dataEnum];
    }
}
