using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeButton : MonoBehaviour {

    [SerializeField] private Pipe pipe;

    private void OnMouseEnter() {
        if (pipe.IfBuilding() || LandPanel.GetInstance().IfAnyPannelShow()) return;
        PipeUIPannel pannel = PipeUIPannel.GetInstance();
        pannel.SetShowPos(transform.position);
        pannel.SetCurPointPipe(pipe);
        pannel.Show();
    }

    private void OnMouseExit() {
        if (pipe.IfBuilding()) return;
        PipeUIPannel.GetInstance().Hide();
    }
}
