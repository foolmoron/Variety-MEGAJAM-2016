using UnityEngine;
using System.Collections;

public class RandomFlip : MonoBehaviour {

    public bool Flipping = false;
    [Range(1, 10)]
    public int FrameInterval = 1;
    int frame;

    void Update() {
        if (Flipping && (frame % FrameInterval == 0)) {
            if (Random.value <= 0.5f) {
                transform.localScale = transform.localScale.timesX(-1);
            } else {
                transform.localScale = transform.localScale.timesY(-1);
            }
        }
        frame++;
    }
}
