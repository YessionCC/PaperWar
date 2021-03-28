using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataRequire : System.EventArgs {

    public PlayerData.PlayerDataEnum dataEnum;
    public int updateVal;//为正数是补充资源，负数是消耗资源

    public PlayerDataRequire(PlayerData.PlayerDataEnum dataEnum, int v) {
        this.dataEnum = dataEnum;
        updateVal = v;
    }
}
