using UnityEngine;
using System.Collections;

public class Player7 : MonoBehaviour {

    [Range(0, 30)]
    public float JumpForce = 8;
    [Range(0, 30)]
    public float Speed = 5;

    public KeyCode UpKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;

    new Rigidbody2D rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        var velocity = rigidbody.velocity;
        if (Input.GetKeyDown(UpKey)) {
            velocity.y += JumpForce;
        }
        if (Input.GetKey(LeftKey)) {
            velocity.x = -Speed;
        } else if (Input.GetKey(RightKey)) {
            velocity.x = Speed;
        } else if (Input.GetKey(UpKey)) {
        }
        rigidbody.velocity = velocity;
    }
}
