using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour {

    public bool Shaking = false;
    [Range(0, 100)]
    public float Strength = 0.05f;
    [Range(1, 10)]
    public int FrameInterval = 1;
    int frame;
    Vector3 previousShake;

    void Update() {
        if (Shaking && (frame % FrameInterval == 0)) {
            transform.localPosition -= previousShake;
            Vector3 shake = Random.insideUnitCircle.normalized * Strength;
            transform.localPosition += shake;
            previousShake = shake;
        }
        frame++;
    }
}
