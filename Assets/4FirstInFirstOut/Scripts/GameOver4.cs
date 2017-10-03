using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameOver4 : MonoBehaviour {

    public bool IsGameOver;
    public KeyCode RestartKey = KeyCode.Space;

    public AudioClip GameOverSound;

    GameObject stuff;

    void Start() {
        stuff = transform.Find("Stuff").gameObject;
    }

    public void DoGameOver() {
        IsGameOver = true;
        Destroy(FindObjectOfType<StackDropper>().gameObject);

        AudioSource.PlayClipAtPoint(GameOverSound, Vector3.zero);
    }

    void Update() {
        if (IsGameOver && Input.GetKeyDown(RestartKey)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        stuff.SetActive(IsGameOver);
    }
}
