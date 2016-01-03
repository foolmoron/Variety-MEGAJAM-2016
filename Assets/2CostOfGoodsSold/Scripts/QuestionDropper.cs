using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class QuestionDropper : MonoBehaviour {

    public event Action OnGameOver = delegate { };

    [Range(0, 10)]
    public float SecondsPerQuestion = 2f;
    float timeSinceLastQuestion;

    public GameObject QuestionPrefab;
    Rect randomDroppingArea;

    [HideInInspector]
    public List<Question> CurrentQuestions = new List<Question>(10);

    [Range(0, 2000)]
    public float Money = 1000;
    float lastMoney;
    [Range(0, 2000)]
    public float MaxMoney = 2000;
    [Range(0, 20)]
    public float MaxMoneyScale = 12;

    public int QuestionsRight;
    public int QuestionsWrong;

    SpriteRenderer bar;
    TextMesh moneyText;
    GameObject gameOver;
    TextMesh scoreText;

    public bool IsGameOver;

    public AudioClip GameOverSound;

    void Start() {
        bar = GetComponentInChildren<SpriteRenderer>();
        moneyText = GetComponentInChildren<TextMesh>();
        gameOver = transform.FindChild("GameOver").gameObject;
        scoreText = gameOver.transform.FindChild("ScoreText").GetComponent<TextMesh>();
        // get random dropping area from the box collider's bounds
        {
            var boxCollider = GetComponent<BoxCollider2D>();
            randomDroppingArea = new Rect(boxCollider.offset - boxCollider.size/2, boxCollider.size);
        }
        Reset();
    }

    public void Reset(float money = 1000) {
        Money = money;
        MaxMoney = money * 1.25f;
        IsGameOver = false;
        timeSinceLastQuestion = SecondsPerQuestion;
        QuestionsRight = 0;
        QuestionsWrong = 0;
    }
    
    void Update() {
        if (!IsGameOver) {
            timeSinceLastQuestion += Time.deltaTime;
        }
        // drop new question
        {
            if (timeSinceLastQuestion >= SecondsPerQuestion) {
                var randomOffset = new Vector2(Random.Range(randomDroppingArea.xMin, randomDroppingArea.xMax), Random.Range(randomDroppingArea.yMin, randomDroppingArea.yMax));
                var question = ((GameObject) Instantiate(QuestionPrefab, transform.position + randomOffset.to3(), Quaternion.identity)).GetComponent<Question>();
                CurrentQuestions.Add(question);
                timeSinceLastQuestion -= SecondsPerQuestion;
            }
        }
        // clean up list of questions
        {
            for (int i = 0; i < CurrentQuestions.Count; i++) {
                var question = CurrentQuestions[i];
                if (question == null) {
                    CurrentQuestions.RemoveAt(i);
                    i--;
                }
            }
        }
        // money can't be negative
        {
            Money = Money < 0 ? 0 : Money;
        }
        // size bar based on money
        {
            var moneyRatio = Money / MaxMoney;
            bar.transform.localScale = bar.transform.localScale.withX(moneyRatio * MaxMoneyScale);
        }
        // update text based on money
        {
            moneyText.text = Money.ToString("C");
        }
        // game over if money is 0
        {
            if (lastMoney > 0 && Money <= 0) {
                IsGameOver = true;
                for (int i = 0; i < CurrentQuestions.Count; i++) {
                    Destroy(CurrentQuestions[i].gameObject);
                }
                CurrentQuestions.Clear();
                scoreText.text = "You got " + QuestionsRight + " questions right!\nYou got " + QuestionsWrong + " questions wrong :(";
                
                AudioSource.PlayClipAtPoint(GameOverSound, Vector3.zero);
                OnGameOver();
            }
        }
        // show game over if necessary
        {
            gameOver.SetActive(IsGameOver);
        }
        lastMoney = Money;
    }
}
