using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBuilder : MonoBehaviour {

    private static PipeBuilder Instance;
    [SerializeField] private Pipe pipePrefab;

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
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pipe.Stretch((Vector2)point);
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(point.x, point.y), Vector2.zero);
                if (hit.collider) {
                    Land land = hit.collider.GetComponent<Land>();
                    if (land != null && 
                        !land.Equals(LandPanel.GetInstance().GetCurPointLand())) {//不建立自己到自己的pipe
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
