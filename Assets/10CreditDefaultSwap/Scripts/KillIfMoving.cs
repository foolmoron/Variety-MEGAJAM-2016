using UnityEngine;
using System.Collections;

public class KillIfMoving : MonoBehaviour {

    public bool LessThan;
    public Vector3 Amount;

    public void OnTriggerEnter2D(Collider2D collision) {
        var moving = collision.GetComponent<Moving>();
        if (moving) {
            if (Amount.x != 0) {
                if ((LessThan && moving.MovementPerSecond.x < Amount.x) || (!LessThan && moving.MovementPerSecond.x >= Amount.x)) {
                    Destroy(moving.gameObject);
                }
            }
            if (Amount.y != 0) {
                if ((LessThan && moving.MovementPerSecond.y < Amount.y) || (!LessThan && moving.MovementPerSecond.y >= Amount.y)) {
                    Destroy(moving.gameObject);
                }
            }
            if (Amount.z != 0) {
                if ((LessThan && moving.MovementPerSecond.z < Amount.z) || (!LessThan && moving.MovementPerSecond.z >= Amount.z)) {
                    Destroy(moving.gameObject);
                }
            }
        }
    }
}
