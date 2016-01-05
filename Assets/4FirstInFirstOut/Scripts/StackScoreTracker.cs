using UnityEngine;
using System.Collections;

public class StackScoreTracker : MonoBehaviour {

    public float Score;

    TextMesh text;

    void Start() {
        text = GetComponentInChildren<TextMesh>();
    }

    void Update() {
        text.text = Score.ToString("0.0m");
    }
}
