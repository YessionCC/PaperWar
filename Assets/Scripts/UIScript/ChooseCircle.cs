using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCircle : MonoBehaviour {

    private static ChooseCircle Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Hide();
    }

    public static ChooseCircle GetInstance() {
        return Instance;
    }

    public void Show(Vector2 position) {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
