using UnityEngine;
using System.Collections;

public class Stack : MonoBehaviour {

    public GameOver4 GameOver;
    public FollowHeight FollowHeight;
    public StackScoreTracker ScoreTracker;
    public bool IsAtTop;
    public float HeightLanded = float.NegativeInfinity;


    public AudioClip ToppingSound;
    public AudioClip CollideSound;

    void Start() {
    }

    void Update() {
        // check if the stack tipped
        {
            if (transform.position.y < (HeightLanded - 1f)) {
                if (!GameOver.IsGameOver) {
                    GameOver.DoGameOver();
                }
            }
        }
    }
    
    public void OnCollisionEnter2D(Collision2D collision) {
        var stack = collision.gameObject.GetComponent<Stack>();
        if (stack && stack.IsAtTop) {
            stack.IsAtTop = false;
            IsAtTop = true;
            HeightLanded = transform.position.y;
            FollowHeight.Height = transform.position.y;
            if (transform.position.y > ScoreTracker.Score) {
                ScoreTracker.Score = transform.position.y;
                AudioSource.PlayClipAtPoint(ToppingSound, Vector3.zero);
            } else {
                AudioSource.PlayClipAtPoint(CollideSound, Vector3.zero);
            }
        } else if (stack) {
            AudioSource.PlayClipAtPoint(CollideSound, Vector3.zero);
        }
    }
}
