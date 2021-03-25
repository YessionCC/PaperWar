using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandEvent : MonoBehaviour {

    private Land land;

    private void Start() {
        land = GetComponent<Land>();
    }

    private void OnMouseEnter() {
        ChooseCircle.GetInstance().Show(transform.position);
    }

    private void OnMouseExit() {
        ChooseCircle.GetInstance().Hide();
    }

}
