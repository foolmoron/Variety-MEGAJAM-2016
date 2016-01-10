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
    float originalAnimationInterval;
    [Range(-0.1f, 0)]
    public float AnimationIntervalVelocity = -0.025f;
    [Range(0, 5)]
    public float Speed = 1f;
    float originalSpeed;
    [Range(0, 0.1f)]
    public float SpeedVelocity = 0.025f; // best variable name ever??

    new BoxCollider2D collider;
    SpriteRenderer sprite;

    public AudioClip StartSound;
    public AudioClip LoseSound;

    void Start() {
        collider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        originalAnimationInterval = AnimationInterval;
        originalSpeed = Speed;
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
                AudioSource.PlayClipAtPoint(LoseSound, Vector3.zero);
            }
        }
        // check for game start or restart
        {
            if (!IsStarted && inBox && (!IsGameOver || pauseTime <= 0)) {
                IsStarted = true;
                IsGameOver = false;
                TimeAlive = 0;
                StartCoroutine(RandomSizings());
                AudioSource.PlayClipAtPoint(StartSound, Vector3.zero);
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
        // velocities
        {
            AnimationInterval = originalAnimationInterval + (AnimationIntervalVelocity * TimeAlive);
            Speed = originalSpeed + (SpeedVelocity * TimeAlive);
        }
    }

    IEnumerator RandomSizings() {
        while (true) {
            yield return new WaitForSeconds(AnimationInterval / 2); // wait between animations
            var randomDirection = Random.insideUnitCircle.normalized;
            var newLocation = new Vector3(Mathf.Lerp(-3, 3, (randomDirection.x / 2 + 0.5f)), Mathf.Lerp(-2, 1.75f, (randomDirection.y / 2 + 0.5f)), 0);
            var duration = (newLocation - transform.position).magnitude / Speed;
            Tween.MoveTo(gameObject, newLocation, duration, Interpolate.EaseType.Linear);
            yield return new WaitForSeconds(duration);
        }
    }
}
