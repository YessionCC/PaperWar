using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandRateUI : MonoBehaviour {

    private Land land;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image belongBar;

    private void Start() {
        land = transform.GetComponentInParent<Land>();
        healthBar.gameObject.SetActive(false);
        land.OnHealthDamage += (object s, EventArgs e) => {
            healthBar.gameObject.SetActive(true);
            healthBar.fillAmount = land.GetHealthRate();
        };
        land.OnBelongDamage += (object s, EventArgs e) => {
            belongBar.color = land.GetAttackerColor();
            belongBar.fillAmount = land.GetBelongRate();
        };
        land.OnBelongPlayerChange += (object s, EventArgs e) => {
            belongBar.color = land.GetPlayer().GetColor();
            ExpandCircle.CreatExpandCircle(transform.position, land.GetPlayer().GetColor());
        };
    }
}
