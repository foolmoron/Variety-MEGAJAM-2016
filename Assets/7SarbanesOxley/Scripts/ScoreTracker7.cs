using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreTracker7 : MonoBehaviour {

    public bool GameOver;

    public Image Timer;
    public Image SarbanesScore;
    [Range(0, 0.25f)]
    public float SarbanesScoreSpeed = 0.1f;
    float desiredSarbanesFill;

    GetColored[] pieces;

    [Range(0, 100)]
    public float TimeLeft = 20;
    float originalTime;

    public GameObject SarbanesWins;
    public GameObject OxleyWins;
    public GameObject Tie;
    public TextMesh SarbanesPerc;
    public TextMesh OxleyPerc;
    public GameObject Rematch;

    void Start() {
        pieces = FindObjectsOfType<GetColored>();
        originalTime = TimeLeft;
    }

    void Update() {
        // timer
        {
            if (!GameOver) {
                TimeLeft -= Time.deltaTime;
                Timer.fillAmount = TimeLeft / originalTime;
            }
        }
        // do sarbanes calculation
        var sarbanesPerc = 0.5f;
        {
            var sarbanes = 0;
            var oxley = 0;
            for (int i = 0; i < pieces.Length; i++) {
                if (pieces[i].IsSarbanes) sarbanes++;
                if (pieces[i].IsOxley) oxley++;
            }
            if (sarbanes + oxley != 0) {
                sarbanesPerc = (float)sarbanes / (sarbanes + oxley);
            }
            desiredSarbanesFill = sarbanesPerc;
        }
        // lerp to desired sarbanes
        {
            SarbanesScore.fillAmount = Mathf.Lerp(SarbanesScore.fillAmount, desiredSarbanesFill, SarbanesScoreSpeed);
        }
        // game over 
        {
            if (TimeLeft <= 0) {
                var colorers = FindObjectsOfType<Colorer>();
                for (int i = 0; i < colorers.Length; i++) {
                    Destroy(colorers[i]);
                }
                GameOver = true;
            }
        }
        // game over texts
        {
            SarbanesWins.SetActive(GameOver && sarbanesPerc > 0.5f);
            OxleyWins.SetActive(GameOver && sarbanesPerc < 0.5f);
            Tie.SetActive(GameOver && sarbanesPerc == 0.5f);
            SarbanesPerc.gameObject.SetActive(GameOver);
            OxleyPerc.gameObject.SetActive(GameOver);
            SarbanesPerc.text = (100 * sarbanesPerc).ToString("F2") + "%";
            OxleyPerc.text = (100 - 100 * sarbanesPerc).ToString("F2") + "%";
            Rematch.SetActive(GameOver);
        }
        // rematch
        {
            if (GameOver && Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
