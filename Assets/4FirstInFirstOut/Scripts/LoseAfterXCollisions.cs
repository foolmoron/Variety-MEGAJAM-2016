using UnityEngine;
using System.Collections;

public class LoseAfterXCollisions : MonoBehaviour {

    [Range(0, 5)]
    public int CollisionsRemaining;

    GameOver4 gameOver;

    void Start() {
        gameOver = FindObjectOfType<GameOver4>();
    }
    
    public void OnCollisionEnter2D(Collision2D collision) {
        CollisionsRemaining--;
        if (CollisionsRemaining <= 0) {
            if (!gameOver.IsGameOver) {
                gameOver.DoGameOver();
            }
        }
    }
}
