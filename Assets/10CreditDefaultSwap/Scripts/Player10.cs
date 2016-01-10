using UnityEngine;
using System.Collections;

public class Player10 : MonoBehaviour {

    [Range(0, 10)]
    public float Speed = 2;
    [Range(0, 0.25f)]
    public float RotationSpeed = 0.1f;

    Vector2 lastDirection;

    new Rigidbody2D rigidbody;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (direction != Vector2.zero) {
            lastDirection = direction;
        }
        // move in direction
        {
            rigidbody.AddForce(direction * Speed);
        }
        // rotate to point to last direction
        {
            var desiredAngle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg - 90;
            transform.localRotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.localRotation.eulerAngles.z, desiredAngle, RotationSpeed));
        }
    }
}
