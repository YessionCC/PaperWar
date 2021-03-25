using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomSensitivity;
    private Camera _camera;
    private Vector3 moveTo;
    private float toOrthographicSize;
    private MoveState moveState;

    private enum MoveState {
        none,
        moveTo,
        zoomInOut,
    }

    private void Start() {
        moveState = MoveState.none;
        _camera = GetComponent<Camera>();
        toOrthographicSize = _camera.orthographicSize;
        LandPanel.GetInstance().OnPointLandChange += (object o, EventArgs e) => {
            Vector3 pos = LandPanel.GetInstance().GetCurPointLand().transform.position;
            moveTo = new Vector3(pos.x, pos.y, transform.position.z);
            moveState = MoveState.moveTo;
        };
    }

    private void Update() {
        switch (moveState) {
            case MoveState.moveTo:
                transform.position = Vector3.Lerp(transform.position, moveTo, moveSpeed);
                if (Vector3.Distance(transform.position, moveTo) <= 0.01f) {
                    transform.position = moveTo;
                    moveState = MoveState.none;
                }
                break;
            case MoveState.zoomInOut:
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, toOrthographicSize, zoomSpeed);
                if (Mathf.Abs(_camera.orthographicSize - toOrthographicSize) <= 0.01f) {
                    _camera.orthographicSize = toOrthographicSize;
                    moveState = MoveState.none;
                }
                break;
            case MoveState.none:
                break;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            float val = Input.GetAxis("Mouse ScrollWheel");
            toOrthographicSize -= val * zoomSensitivity;
            toOrthographicSize = Mathf.Clamp(toOrthographicSize, 4, 12);
            moveState = MoveState.zoomInOut;
        }
        if (Input.GetAxis("Horizontal") != 0) {
            float val = Input.GetAxis("Horizontal");
            moveTo = new Vector3(val*10, 0, 0) + transform.position;
            moveState = MoveState.moveTo;
        }
        if (Input.GetAxis("Vertical") != 0) {
            float val = Input.GetAxis("Vertical");
            moveTo = new Vector3(0, val*10, 0) + transform.position;
            moveState = MoveState.moveTo;
        }

    }

}
