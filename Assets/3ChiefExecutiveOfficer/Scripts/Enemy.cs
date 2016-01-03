using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision) {
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
