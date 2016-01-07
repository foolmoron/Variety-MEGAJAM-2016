using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliceColorer : MonoBehaviour {

    public Image[] Slices;
    
    public Color InitialColor;

    void Awake() {
        // just set colors at start since they won't change
        var hsb = HSBColor.FromColor(InitialColor);
        for (int i = 0; i < Slices.Length; i++) {
            var slice = Slices[i];
            var lerp = (float)i / Slices.Length;
            slice.color = new HSBColor((hsb.h + lerp) % 1, hsb.s, hsb.b, hsb.a).ToColor();
        }
    }
}