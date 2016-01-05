using UnityEngine;
using System.Collections;

public class StackDropper : MonoBehaviour {

    [Range(0, 5)]
    public float HueSpeed;

    public GameObject DropPrefab;

    HSBColor currentColor;
    SpriteRenderer sprite;

    GameOver4 gameOver;
    FollowHeight followHeight;
    StackScoreTracker scoreTracker;

    void Start() {
        gameOver = FindObjectOfType<GameOver4>();
        followHeight = FindObjectOfType<FollowHeight>();
        scoreTracker = FindObjectOfType<StackScoreTracker>();
        sprite = GetComponent<SpriteRenderer>();
        currentColor = HSBColor.FromColor(sprite.color);
    }
    void Update() {
        // rotate hue
        {
            currentColor.h = (currentColor.h + HueSpeed * Time.deltaTime) % 1;
            sprite.color = currentColor.ToColor();
        }
        // drop when clicked
        {
            if (Input.GetMouseButtonDown(0)) {
                var drop = Instantiate(DropPrefab);
                drop.transform.position = transform.position;
                drop.transform.rotation = transform.rotation;
                drop.transform.localScale = transform.localScale;

                var dropStack = drop.GetComponent<Stack>();
                dropStack.GameOver = gameOver;
                dropStack.FollowHeight = followHeight;
                dropStack.ScoreTracker = scoreTracker;

                var dropSprite = drop.GetComponent<SpriteRenderer>();
                dropSprite.color = sprite.color.withAlpha(1);
            }
        }
    }
}
