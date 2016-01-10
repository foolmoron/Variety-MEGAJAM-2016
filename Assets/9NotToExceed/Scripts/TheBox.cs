using UnityEngine;
using System.Collections;

public class TheBox : MonoBehaviour {

    public bool IsStarted;
    public bool IsGameOver;

    public GameObject GetInText;
    public GameObject StayText;
    public GameObject LoseText;
    public TextMesh ScoreText;
    public GameObject RestartText;

    public float TimeAlive;

    [Range(0, 5)]
    public float PauseAfterLose = 2f;
    float pauseTime;

    [Range(0, 5)]
    public float AnimationInterval = 3f;
    [Range(0, 5)]
    public float Speed = 1f;

    new BoxCollider2D collider;
    SpriteRenderer sprite;

    void Start() {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update() {
        // is in box?
        var inBox = false;
        {
            if (collider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
                inBox = true;
            }
        }
        // check for loss
        {
            if (!IsGameOver && IsStarted && !inBox) {
                IsGameOver = true;
                IsStarted = false;
                pauseTime = PauseAfterLose;
                Tween.Stop(gameObject);
                transform.position = Vector3.zero;
                transform.localScale = Vector3.one;
                StopAllCoroutines();
                
            }
        }
        // check for game start or restart
        {
            if (!IsStarted && inBox && (!IsGameOver || pauseTime <= 0)) {
                IsStarted = true;
                IsGameOver = false;
                TimeAlive = 0;
                StartCoroutine(RandomSizings());
            }
        }
        // add time if started 
        {
            if (IsStarted && !IsGameOver) {
                TimeAlive += Time.deltaTime;
            }
        }
        // pause after lose
        {
            if (pauseTime > 0) {
                pauseTime -= Time.deltaTime;
            }
        }
        // toggle stuff
        {
            GetInText.SetActive(!IsGameOver && !IsStarted);
            StayText.SetActive(!IsGameOver && IsStarted);
            LoseText.SetActive(IsGameOver);
            ScoreText.gameObject.SetActive(IsGameOver);
            ScoreText.text = "you were in there for\n" + TimeAlive.ToString("0.00") + " seconds";
            RestartText.SetActive(IsGameOver && pauseTime <= 0);
            sprite.enabled = pauseTime <= 0;
        }
    }

    IEnumerator RandomSizings() {
        while (true) {
            yield return new WaitForSeconds(AnimationInterval / 2); // wait between animations
            var randomDirection = Random.insideUnitCircle.normalized;
            Debug.Log(randomDirection);
            var newLocation = new Vector3(Mathf.Lerp(-3, 3, (randomDirection.x / 2 + 0.5f)), Mathf.Lerp(-2, 1.75f, (randomDirection.y / 2 + 0.5f)), 0);
            var duration = (newLocation - transform.position).magnitude / Speed;
            Tween.MoveTo(gameObject, newLocation, duration, Interpolate.EaseType.Linear);
            yield return new WaitForSeconds(duration);
        }
    }
}
