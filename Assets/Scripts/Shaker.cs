using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour {

    public bool Shaking = false;
    [Range(0, 1)]
    public float Strength = 0.05f;
    [Range(1, 10)]
    public int FrameInterval = 1;
    Vector3 previousShake;

    void Update() {
        if (Shaking) {
            transform.localPosition -= previousShake;
            Vector3 shake = Random.insideUnitCircle.normalized * Strength;
            transform.localPosition += shake;
            previousShake = shake;
        }
    }
}
