using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    [SerializeField] private float maxHealth;
    private float curHealth;
    public EventHandler OnHealthChange;
    public EventHandler OnHealthZero;

    private bool ifStartRecover;
    [SerializeField] private float recoverSpeed;

    private void Start() {
        curHealth = maxHealth;
        ifStartRecover = false;
    }

    public void StartRecover() {
        if (ifStartRecover) return;
        ifStartRecover = true;
        StartCoroutine("Recover");
    }

    public void StopRecover() {
        if (!ifStartRecover) return;
        ifStartRecover = false;
        StopAllCoroutines();
    }

    public void Damage(float damage) {
        curHealth -= damage;
        if (curHealth < 0) {
            curHealth = 0;
            if (OnHealthZero != null)
                OnHealthZero.Invoke(this, EventArgs.Empty);
        }
        if (OnHealthChange != null)
            OnHealthChange.Invoke(this, EventArgs.Empty);
    }

    IEnumerator Recover() {
        while (true) {
            curHealth += recoverSpeed;
            if (curHealth > maxHealth) curHealth = maxHealth;
            if (OnHealthChange != null) OnHealthChange.Invoke(this, EventArgs.Empty);
            yield return new WaitForSeconds(0.2f);
        }
    }

}
