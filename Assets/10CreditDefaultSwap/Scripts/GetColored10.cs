using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GetColored10 : MonoBehaviour {

    public bool Inverted;

    Colorizer10 colorizer;
    SpriteRenderer sprite;
    
    void Start() {
        colorizer = Colorizer10.instance;
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update() {
        var alpha = sprite.color.a;
        if (Application.isPlaying) {
            sprite.color = Inverted ? colorizer.RGBInverted.withAlpha(alpha) : colorizer.RGBColor.withAlpha(alpha);
        } else {
            sprite.color = Inverted ? Color.black.withAlpha(alpha) : Color.white.withAlpha(alpha);
        }
    }
}
