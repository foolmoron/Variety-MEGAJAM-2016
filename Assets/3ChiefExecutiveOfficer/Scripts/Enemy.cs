using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public Transform Target;
    [Range(0, 10)]
    public float Speed = 1;

    void Update() {
        // approach target
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        var bullet = collision.GetComponent<Bullet>();
        if (bullet) {
            Destroy(bullet.gameObject);
            Destroy(gameObject);
        }

        var player = collision.GetComponent<TwinStick>();
        if (player) {
            // TODO: gameover
            Destroy(gameObject);
        }
    }
}
