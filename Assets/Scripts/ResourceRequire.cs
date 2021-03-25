using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceRequire: System.EventArgs {

    public ResourcesEnum resource;
    public int updateVal;//为正数是补充资源，负数是消耗资源

}
