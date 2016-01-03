using UnityEngine;
using System.Collections;

public class DieAtY : MonoBehaviour {

    public float YToDieAt;
    public GameObject DiePrefab;
    public Vector3 DiePrefabOffset;

    public AudioClip BadSound;

    QuestionDropper questionDropper;

    void Start() {
        questionDropper = FindObjectOfType<QuestionDropper>();
    }

    void Update() {
        if (transform.position.y <= YToDieAt) {
            var text = ((GameObject)Instantiate(DiePrefab, transform.position + DiePrefabOffset, Quaternion.identity)).GetComponent<TextMesh>();
            var question = GetComponent<Question>();
            text.text = "-$" + question.WholePart + "." + question.DecimalTensPart + "0";
            Destroy(gameObject);
            questionDropper.Money -= question.GetSolution();
            questionDropper.QuestionsWrong++;
            AudioSource.PlayClipAtPoint(BadSound, Vector3.zero);
        }
    }
}
