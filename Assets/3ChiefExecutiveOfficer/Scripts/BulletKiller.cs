using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletKiller : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision) {
        var bullet = collision.GetComponent<Bullet>();
        if (bullet) {
            Destroy(bullet.gameObject);
        }
    }
}
