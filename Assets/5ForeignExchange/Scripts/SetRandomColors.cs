using UnityEngine;
using System.Collections;

public class SetRandomColors : MonoBehaviour {

    [Range(0, 1)]
    public float Saturation = 0.5f;
    public SpriteRenderer sprite1;
    public TextMesh text1;
    public TextMesh text1b;
    public TextMesh text2;

    void Start() {
        var color = new HSBColor(Random.value, Saturation, 1);
        sprite1.color = color.ToColor();
        text1.color = color.ToColor();
        text1b.color = color.ToColor();
        var color2 = new HSBColor((color.h + 0.5f) % 1, Saturation, 1);
        text2.color = color2.ToColor();
    }
}