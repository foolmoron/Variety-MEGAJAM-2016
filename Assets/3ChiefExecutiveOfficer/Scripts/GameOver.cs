using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public bool IsGameOver;
    public KeyCode RestartKey = KeyCode.Space;

    [Range(0, 100)]
    public float Bloodied;
    [Range(0, 100)]
    public float VeryBloodied;
    [Range(0, 100)]
    public float Clean;

    public float TimeAlive;

    GameObject stuff;
    Shaker shaker;
    [Range(0, 0.2f)]
    public float ExecutionShakerMax = 0.1f;

    GameObject peaceText;
    GameObject executionText;
    TextMesh resetText;

    TextMesh bloodiedText;
    TextMesh veryBloodiedText;
    TextMesh cleanText;
    TextMesh timeText;

    void Start() {
        stuff = transform.FindChild("Stuff").gameObject;
        shaker = GetComponentInChildren<Shaker>();
        peaceText = transform.FindChild("Stuff/PeaceText").gameObject;
        executionText = transform.FindChild("Stuff/ExecutionText").gameObject;
        resetText = transform.FindChild("Stuff/ResetText").GetComponent<TextMesh>();
        bloodiedText = transform.FindChild("Stuff/BText").GetComponent<TextMesh>();
        veryBloodiedText = transform.FindChild("Stuff/VBText").GetComponent<TextMesh>();
        cleanText = transform.FindChild("Stuff/CText").GetComponent<TextMesh>();
        timeText = transform.FindChild("Stuff/TText").GetComponent<TextMesh>();
    }

    public void DoGameOver() {
        IsGameOver = true;
        Destroy(FindObjectOfType<TwinStick>().gameObject);
        var enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++) {
            Destroy(enemies[i].gameObject);
        }
    }

    void Update() {
        if (IsGameOver && Input.GetKeyDown(RestartKey)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        stuff.SetActive(IsGameOver);
        if (!IsGameOver) {
            TimeAlive += Time.deltaTime;
        }
        // header text
        {
            peaceText.SetActive(Clean >= 100);
            executionText.SetActive(Clean < 100);
            shaker.Strength = ExecutionShakerMax * (Bloodied + VeryBloodied * 2) / 200;
        }
        // score texts
        {
            bloodiedText.text = Bloodied.ToString("0.#") + "% Bloodied";
            veryBloodiedText.text = VeryBloodied.ToString("0.#") + "% VERY Bloodied";
            cleanText.text = Clean.ToString("0.#") + "% Clean";
            timeText.text = TimeAlive.ToString("0.00") + "s Alive";
        }
        // reset text
        {
            resetText.text = "Press SPACE to restart your " + (Clean >= 100 ? "peace" : "murder");
        }
    }
}
