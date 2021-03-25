using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {

    private Land toLand;
    private Land fromLand;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform line;

    public void SetFrom(Land from) {
        fromLand = from;
    }

    public void SetToLand(Land to) {
        toLand = to;
        Stretch(to.GetEdgePosition(fromLand.transform.position));
        to.AddPipeFrom(fromLand);
    }

    public void Stretch(Vector3 pos) {
        transform.position = fromLand.GetEdgePosition(pos);

        Vector3 posz0 = (Vector2)pos;
        Vector3 dir = posz0 - transform.position;

        if (Vector3.Dot(dir, transform.position - fromLand.transform.position) < 0)//避免箭头方向乱
            gameObject.SetActive(false);
        else gameObject.SetActive(true);

        Vector3 midPoint = 0.5f*(posz0 + transform.position);
        arrow.position = midPoint;

        float dis = dir.magnitude;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
        line.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        arrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        line.localScale = new Vector3(dis*2, 1, 1);
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
