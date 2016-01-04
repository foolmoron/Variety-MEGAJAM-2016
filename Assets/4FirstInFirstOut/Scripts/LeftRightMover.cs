using UnityEngine;
using System.Collections;

public class LeftRightMover : MonoBehaviour {

    [Range(0, 10)]
    public float Speed = 5;

    public float MinX = -2.5f;
    public float MaxX = 2.5f;
    public bool MovingRight;

    void Update() {
        // move back and forth
        {
            if (MovingRight) {
                var newX = Mathf.MoveTowards(transform.position.x, MaxX, Speed * Time.deltaTime);
                transform.position = transform.position.withX(newX);
                if (newX >= MaxX) {
                    MovingRight = false;
                }
            } else {
                var newX = Mathf.MoveTowards(transform.position.x, MinX, Speed * Time.deltaTime);
                transform.position = transform.position.withX(newX);
                if (newX <= MinX) {
                    MovingRight = true;
                }
            }
        }
    }
}
