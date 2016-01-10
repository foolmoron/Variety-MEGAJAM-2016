using UnityEngine;
using System.Collections;

public class GameOverWhenHitPlayer : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        var player = collision.GetComponent<Player10>();
        if (player && collision.isTrigger) {
            player.GameOver();
        }
    }
}
