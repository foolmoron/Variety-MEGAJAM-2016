using UnityEngine;
using System.Collections;

public class StackDropper : MonoBehaviour {

    public GameObject DropPrefab;

    SpriteRenderer sprite;
    
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update() {
        // drop when clicked
        {
            if (Input.GetMouseButtonDown(0)) {
                var drop = Instantiate(DropPrefab);
                drop.transform.position = transform.position;
                drop.transform.rotation = transform.rotation;
                drop.transform.localScale = transform.localScale;
                var dropSprite = drop.GetComponent<SpriteRenderer>();
                dropSprite.color = sprite.color;
            }
        }
    }
}
