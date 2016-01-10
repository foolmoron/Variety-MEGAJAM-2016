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
                transform.position = Vector3.zero;
                transform.localScale = Vector3.one * 0.5f;
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
            Debug.Log("x");
            yield return new WaitForSeconds(2f);
        }
    }
}
