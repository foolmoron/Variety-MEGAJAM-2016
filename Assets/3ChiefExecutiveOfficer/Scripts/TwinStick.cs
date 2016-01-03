using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwinStick : MonoBehaviour {

    [Range(0, 10)]
    public float Speed = 2;

    public GameObject BulletPrefab;
    
    void Start() {
    
    }
    
    void Update() {
        // move based on WASD
        {
            var direction = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) direction.y = 1;
            if (Input.GetKey(KeyCode.S)) direction.y = -1;
            if (Input.GetKey(KeyCode.D)) direction.x = 1;
            if (Input.GetKey(KeyCode.A)) direction.x = -1;
            transform.position += direction.normalized.to3() * Speed * Time.deltaTime;
        }
    }
}
