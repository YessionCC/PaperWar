using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {

    private Land toLand;
    private Land fromLand;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform line;
    [SerializeField] private SpriteRenderer spriteLine;
    [SerializeField] private Color accessColor;
    [SerializeField] private Color denyColor;

    private PipeManager fromMan;
    private PipeManager toMan;

    private Vector3 fromPos;
    private Vector3 toPos;

    private Player player;

    private bool isBuilding;//当前是否正在被建造

    private void Start() {
        isBuilding = true;
    }

    public void SetFrom(Land from) {
        fromLand = from;
        fromMan = fromLand.GetPipeManager();
        player = fromLand.GetPlayer();
    }

    public void SetToLand(Land to) {
        toLand = to;
        Stretch(to.GetEdgePosition(fromLand.transform.position), true);

        toMan = toLand.GetPipeManager();
        fromPos = fromLand.GetEdgePosition(toLand.transform.position);
        toPos = toLand.GetEdgePosition(fromLand.transform.position);

        toMan.AddPipeFrom(this);
        fromMan.AddPipeTo(this);
        isBuilding = false;
    }

    public void Stretch(Vector3 pos, bool isAccess) {
        if (isAccess) spriteLine.color = accessColor;
        else spriteLine.color = denyColor;
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

    public void ForwardRequests(ResourceRequire resourceRequire, int forwardNum) {
        fromMan.SendResources(resourceRequire, this, forwardNum);
        Debug.DrawLine(toPos, fromPos, Color.red);
    }

    public void SendResources(ResourceRequire resourceRequire) {
        StartCoroutine("SendResourcesOneByOne", resourceRequire);
    }

    IEnumerator SendResourcesOneByOne(ResourceRequire require) {
        for (int i = 0; i < -require.updateVal; i++) {//一次发一个
            Resource.GetInstance().SendResource(fromPos, toPos, player,  toMan.GetResourcesManager(), require.resource);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void Disconnect() {
        toMan.RemovePipeFrom(this);
        fromMan.RemovePipeFrom(this);
        Destroy(gameObject);
    }

    public bool IfBuilding() {
        return isBuilding;
    }

    public Player GetPlayer() {
        return player;
    }

    public float GetLength(Vector3 pos) {
        return Vector3.Distance(pos, fromLand.transform.position);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
