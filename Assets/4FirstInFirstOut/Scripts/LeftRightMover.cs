using UnityEngine;
using System.Collections;

public class LeftRightMover : MonoBehaviour {

    [Range(0, 10)]
    public float MinSpeed = 4.5f;
    [Range(0, 10)]
    public float MaxSpeed = 6f;
    [Range(0, 10)]
    public float Speed = 5f;

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
                    Speed = Mathf.Lerp(MinSpeed, MaxSpeed, Random.value);
                }
            } else {
                var newX = Mathf.MoveTowards(transform.position.x, MinX, Speed * Time.deltaTime);
                transform.position = transform.position.withX(newX);
                if (newX <= MinX) {
                    MovingRight = true;
                    Speed = Mathf.Lerp(MinSpeed, MaxSpeed, Random.value);
                }
            }
        }
    }
}
