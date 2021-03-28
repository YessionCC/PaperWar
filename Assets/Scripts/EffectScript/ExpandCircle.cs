using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandCircle : MonoBehaviour {

    private Color color;
    private SpriteRenderer sprite;

    public static void CreatExpandCircle(Vector3 pos, Color col) {
        ExpandCircle circle = Instantiate(Resource.GetInstance().expandCircle, pos, Quaternion.identity);
        circle.color = col;
        circle.sprite = circle.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(5, 5, 0), 0.05f);
        sprite.color = new Color(color.r, color.g, color.b, Mathf.Lerp(sprite.color.a, 0, 0.05f));
        if (Mathf.Abs(sprite.color.a) <= 0.001f) {
            Destroy(gameObject);
        }
    }
}
