using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestionDropper : MonoBehaviour {

    [Range(0, 10)]
    public float SecondsPerQuestion = 2f;
    float timeSinceLastQuestion;

    public GameObject QuestionPrefab;
    Rect randomDroppingArea;

    [HideInInspector]
    public List<Question> CurrentQuestions = new List<Question>(10);

    [Range(0, 2000)]
    public float Money = 1000;
    [Range(0, 2000)]
    public float MaxMoney = 2000;
    [Range(0, 20)]
    public float MaxMoneyScale = 12;

    SpriteRenderer bar;
    TextMesh moneyText;

    public bool IsGameOver;

    void Start() {
        bar = GetComponentInChildren<SpriteRenderer>();
        moneyText = GetComponentInChildren<TextMesh>();
        // get random dropping area from the box collider's bounds
        {
            var boxCollider = GetComponent<BoxCollider2D>();
            randomDroppingArea = new Rect(boxCollider.offset - boxCollider.size/2, boxCollider.size);
        }
        timeSinceLastQuestion = SecondsPerQuestion;
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
        // size bar based on money
        {
            var moneyRatio = Money / MaxMoney;
            moneyRatio = moneyRatio < 0 ? 0 : moneyRatio;
            bar.transform.localScale = bar.transform.localScale.withX(moneyRatio * MaxMoneyScale);
        }
        // update text based on money
        {
            moneyText.text = Money.ToString("C");
        }
        // game over if money is 0
        {
            if (Money <= 0) {
                for (int i = 0; i < CurrentQuestions.Count; i++) {
                    Destroy(CurrentQuestions[i].gameObject);
                }
                CurrentQuestions.Clear();
            }
        }
    }
}
