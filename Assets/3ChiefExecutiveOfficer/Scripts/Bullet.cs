using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

    [Range(0, 30)]
    public float Speed = 2;

    void Update() {
        // move
        {
            transform.Translate(new Vector3(Speed * Time.deltaTime, 0, 0));
        }
    }
}
