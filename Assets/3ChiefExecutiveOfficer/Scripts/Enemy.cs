using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public Transform Target;
    [Range(0, 10)]
    public float Speed = 1;

    void Start() {
        GetComponent<SpriteRenderer>().color = new HSBColor(Random.value, 0.5f, 1f).ToColor();
    }

    void Update() {
        // approach target manhattan style
        {
            var directionToTarget = (Target.position - transform.position).to2().normalized;
            if (Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.y)) {
                transform.position = transform.position.plusX(Mathf.Sign(directionToTarget.x) * Speed * Time.deltaTime);
            } else {
                transform.position = transform.position.plusY(Mathf.Sign(directionToTarget.y) * Speed * Time.deltaTime);
            }
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
