using UnityEngine;
using System.Collections;

public class TheBox : MonoBehaviour {

    public bool IsStarted;
    public bool IsGameOver;

    public GameObject GetInText;
    public GameObject StayText;
    public GameObject LoseText;
    public TextMesh ScoreText;

    public float TimeAlive;

    new BoxCollider2D collider;

    void Start() {
        collider = GetComponent<BoxCollider2D>();
    }
    
    void Update() {
        // is in box?
        var inBox = false;
        {
            if (collider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
                inBox = true;
            }
        }
        // check for game start
        {
            if (!IsStarted && inBox) {
                IsStarted = true;
            }
        }
        // check if lost
        {
            if (IsStarted && !inBox) {
                IsGameOver = true;
            }
        }
        // add time if started 
        {
            if (IsStarted && !IsGameOver) {
                TimeAlive += Time.deltaTime;
            }
        }
        // toggle texts
        {
            GetInText.SetActive(!IsGameOver && !IsStarted);
            StayText.SetActive(!IsGameOver && IsStarted);
            LoseText.SetActive(IsGameOver);
            ScoreText.gameObject.SetActive(IsGameOver);
            ScoreText.text = "you were in the there for\n" + TimeAlive.ToString("0.00") + " seconds";
        }
    }
}
