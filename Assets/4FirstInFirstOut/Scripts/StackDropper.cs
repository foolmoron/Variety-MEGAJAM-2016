using UnityEngine;
using System.Collections;

public class StackDropper : MonoBehaviour {

    [Range(0, 5)]
    public float HueSpeed;

    public GameObject DropPrefab;

    HSBColor currentColor;
    SpriteRenderer sprite;
    
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        currentColor = HSBColor.FromColor(sprite.color);
    }
    void Update() {
        // rotate hue
        {
            currentColor.h = (currentColor.h + HueSpeed * Time.deltaTime) % 1;
            sprite.color = currentColor.ToColor();
        }
        // drop when clicked
        {
            if (Input.GetMouseButtonDown(0)) {
                var drop = Instantiate(DropPrefab);
                drop.transform.position = transform.position;
                drop.transform.rotation = transform.rotation;
                drop.transform.localScale = transform.localScale;
                var dropSprite = drop.GetComponent<SpriteRenderer>();
                dropSprite.color = sprite.color.withAlpha(1);
            }
        }
    }
}
