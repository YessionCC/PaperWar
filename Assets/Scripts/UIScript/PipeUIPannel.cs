using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeUIPannel : MonoBehaviour, IPannelInterface {

    private static PipeUIPannel Instance;
    private Pipe curPointPipe;
    private Vector3 showPos;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Hide();
    }

    public static PipeUIPannel GetInstance() {
        return Instance;
    }

    public void SetCurPointPipe(Pipe pipe) {
        curPointPipe = pipe;
    }

    public void SetShowPos(Vector3 pos) {
        showPos = pos;
    }

    public void Show() {
        transform.position = showPos;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Disconnect() {
        curPointPipe.Disconnect();
        Hide();
    }
}
