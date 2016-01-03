using UnityEngine;
using System.Collections;

public class SpriteSwapper : MonoBehaviour {

    public Sprite Sprite1;
    [Range(0, 3)]
    public float Sprite1Time;
    public Sprite Sprite2;
    [Range(0, 3)]
    public float Sprite2Time;
    bool currently1 = true;
    float time;

    SpriteRenderer sprite;

    void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        time += Time.deltaTime;
        if ((currently1 && time >= Sprite1Time) || (!currently1 && time >= Sprite2Time)) {
            currently1 = !currently1;
            sprite.sprite = currently1 ? Sprite1 : Sprite2;
            time = 0;
        }
    }
}
