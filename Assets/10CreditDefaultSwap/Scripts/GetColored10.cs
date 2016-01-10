using UnityEngine;
using System.Collections;

public class GetColored10 : MonoBehaviour {

    Colorizer10 colorizer;
    SpriteRenderer sprite;
    
    void Start() {
        colorizer = Colorizer10.instance;
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update() {
        sprite.color = colorizer.RGBColor;
    }
}
