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

    void Start() {
        // get random dropping area from the box collider's bounds
        {
            var boxCollider = GetComponent<BoxCollider2D>();
            randomDroppingArea = new Rect(boxCollider.offset - boxCollider.size/2, boxCollider.size);
        }
    }

    public void TrySolution(float solution) {
        for (int i = 0; i < CurrentQuestions.Count; i++) {
            var question = CurrentQuestions[i];
            if (solution == question.GetSolution()) {
                Destroy(question.gameObject);
                CurrentQuestions.RemoveAt(i);
                i--;
            }
        }
    }
    
    void Update() {
        timeSinceLastQuestion += Time.deltaTime;
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
    }
}
