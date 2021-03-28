using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLocator : MonoBehaviour {

    private static ShootLocator Instance;
    private void Awake() {
        Instance = this;
    }
    public static ShootLocator GetInstance() {
        return Instance;
    }

    private Shooter shooter;
    private Land curLand;
    [SerializeField] private GameObject locatorSprite;

    private void Start() {
        locatorSprite.SetActive(false);
    }

    public void Shoot() {
        curLand = LandPanel.GetInstance().GetCurPointLand();
        shooter = curLand.GetBuilding().GetComponent<Shooter>();
        StartCoroutine("SelectToLand", shooter);
        MouseStateRecorder.GetInstance().SetCurMouseState(MouseStateRecorder.MouseState.emitBullet);
    }

    IEnumerator SelectToLand(Shooter shooter) {
        locatorSprite.SetActive(true);
        bool isLocate = false;
        while (true) {
            locatorSprite.transform.Rotate(Vector3.forward, 1.0f);
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            locatorSprite.transform.position = (Vector2)point;
            shooter.TurretLookAt(point);
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(point.x, point.y), Vector2.zero);
                if (hit.collider) {
                    Land land = hit.collider.GetComponent<Land>();
                    if (land != null && !land.GetPlayer().Equals(curLand.GetPlayer()) && shooter.IfDisWithin(point)) {
                        locatorSprite.transform.position = land.transform.position;
                        shooter.AutoShoot(land);
                        isLocate = true;
                        break;
                    }
                }
                break;
            }
            yield return 0;
        }
        MouseStateRecorder.GetInstance().SetCurMouseState(MouseStateRecorder.MouseState.normal);
        if (isLocate) yield return new WaitForSeconds(1f);
        locatorSprite.SetActive(false);
    }
}
