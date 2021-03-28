using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBuilder : MonoBehaviour {

    private static PipeBuilder Instance;
    [SerializeField] private Pipe pipePrefab;
    [SerializeField] private float maxPipeLen;
    [SerializeField] private float pipeCostPerLen;

    private void Awake() {
        Instance = this;
    }

    public static PipeBuilder GetInstance() {
        return Instance;
    }

    public void BuildPipe() {
        Land landFrom = LandPanel.GetInstance().GetCurPointLand();
        Pipe pipe = Instantiate(pipePrefab.gameObject, landFrom.transform.position, Quaternion.identity).GetComponent<Pipe>();
        pipe.SetFrom(landFrom);
        StartCoroutine("SelectToLand", pipe);
        MouseStateRecorder.GetInstance().SetCurMouseState(MouseStateRecorder.MouseState.buildPipe);
    }

    IEnumerator SelectToLand(Pipe pipe) {
        while (true) {
            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pipe.Stretch(point, pipe.GetLength(point)<maxPipeLen);
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
                if (hit.collider) {
                    Land land = hit.collider.GetComponent<Land>();
                    float dis = 0;
                    if (land != null &&
                        !land.Equals(LandPanel.GetInstance().GetCurPointLand()) &&  //不建立自己到自己的pipe
                        pipe.GetPlayer().Equals(land.GetPlayer()) &&    //不建立自己到别人的pipe
                        (dis = pipe.GetLength(point)) < maxPipeLen) {  //不能太长
                        PlayerDataRequire dataRequire = new PlayerDataRequire(PlayerData.PlayerDataEnum.capital, -(int)(pipeCostPerLen * dis));
                        bool ret = pipe.GetPlayer().GetData().UpdateData(dataRequire, false);
                        if (!ret) break; //资金不够
                        pipe.GetPlayer().GetData().UpdateData(dataRequire, true);
                        pipe.SetToLand(land);
                        break;
                    }
                }
                Destroy(pipe.gameObject);
                break;
            }
            yield return 0;
        }
        MouseStateRecorder.GetInstance().SetCurMouseState(MouseStateRecorder.MouseState.normal);
    }

}
