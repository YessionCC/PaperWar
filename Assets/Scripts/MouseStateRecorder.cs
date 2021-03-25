using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateRecorder : MonoBehaviour {

    private MouseState mouseState;
    private event EventHandler OnMouseStateChange;
    private static MouseStateRecorder Instance;

    public enum MouseState {
        normal,
        buildPipe,

    }

    private void Awake() {
        Instance = this;
    }

    void Start () {
        mouseState = MouseState.normal;
	}
	
	public static MouseStateRecorder GetInstance() {
        return Instance;
    }

    public void SetCurMouseState(MouseState state) {
        if (state == mouseState) return;
        mouseState = state;
        if (OnMouseStateChange != null)
            OnMouseStateChange.Invoke(this, EventArgs.Empty);
    }

    public MouseState GetCurMouseState() {
        return mouseState;
    }
}
