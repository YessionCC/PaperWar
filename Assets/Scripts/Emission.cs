using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emission : MonoBehaviour {

    public event EventHandler OnArrive;
    private Vector3 toWhere;
    private float speed;
    private Player player;
    
    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void SetToWhere(Vector3 to) {
        toWhere = to;
    }

    public void SetPlayer(Player player) {
        this.player = player;
    }

	void Update () {
        Vector2 dir = toWhere - transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.position = Vector2.MoveTowards(transform.position, toWhere, speed);
        if(Vector2.Distance(transform.position, toWhere) < 0.01f) {
            if (OnArrive != null) OnArrive.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
            
	}
}
