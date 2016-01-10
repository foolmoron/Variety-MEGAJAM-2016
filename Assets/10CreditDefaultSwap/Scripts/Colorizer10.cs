using UnityEngine;
using System.Collections;

public class Colorizer10 : MonoBehaviour {

    public static Colorizer10 instance;

    public HSBColor color;
    public Color RGBColor;
    public Color RGBInverted;
    [Range(0, 5)]
    public float HueVelocity = 1;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    void Update() {
        color.h  = (color.h + HueVelocity * Time.deltaTime) % 1;
        RGBColor = color.ToColor();
        RGBInverted = new Color(1 - RGBColor.r, 1 - RGBColor.g, 1 - RGBColor.b, RGBColor.a);
    }
}
