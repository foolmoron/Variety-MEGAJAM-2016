using UnityEngine;
using System.Collections;

public class MoveThenDie : MonoBehaviour {

    public AnimationCurve YMovement;
    public float MovementTime;
    Vector3 originalPos;

    void Start() {
        originalPos = transform.position;
    }
    
    void Update() {
        MovementTime += Time.deltaTime;
        // move based on curve
        {
            var movementY = YMovement.Evaluate(MovementTime);
            transform.position = originalPos.plusY(movementY);
        }
        // die after reaching end
        {
            if (MovementTime > YMovement.keys[YMovement.length - 1].time) {
                Destroy(gameObject);
            }
        }
    }
}
