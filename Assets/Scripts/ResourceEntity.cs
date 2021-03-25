using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceEntity : MonoBehaviour {

    public event EventHandler OnArrive;
    private Vector3 toWhere;
    private float speed;
    
    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void SetToWhere(Vector3 to) {
        toWhere = to;
    }

	void Update () {
        transform.position = Vector2.MoveTowards(transform.position, toWhere, speed);
        if(Vector2.Distance(transform.position, toWhere) < 0.01f) {
            if (OnArrive != null) OnArrive.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
            
	}
}
