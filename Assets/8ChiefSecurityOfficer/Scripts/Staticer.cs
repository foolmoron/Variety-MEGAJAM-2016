using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Staticer : MonoBehaviour {

    [Range(0, 1)]
    public float Amount;
    [Range(0, 30)]
    public int OnFrames;
    public Image Static1;
    public Image Static2;

    void Update() {
        if (OnFrames > 0) {
            Static1.color = Color.white;
            Static2.color = Color.white;
            OnFrames--;
        } else {
            Static1.color = Color.white.withAlpha(Amount);
            Static2.color = Color.white.withAlpha(Amount);
        }
    }
}
