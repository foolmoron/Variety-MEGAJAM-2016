using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Returner : MonoBehaviour {

    [Range(0, 5)]
    public float TimeToHold = 3;
    float timeHeld;
    GameObject stuff;

    Shaker shaker;
    float initialShake;
    
    void Start() {
        // check if another exists, and delete self if necessary
        if (FindObjectOfType<Returner>() != this) {
            Destroy(gameObject);
            return;
        }
        // keep alive always
        DontDestroyOnLoad(gameObject);
        // init stuff
        stuff = transform.FindChild("Stuff").gameObject;
        shaker = GetComponentInChildren<Shaker>();
        initialShake = shaker.Strength;
        stuff.SetActive(false);
    }
    
    void Update() {
        // don't do anything on main menu
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            return;
        }

        // position on camera
        {
            transform.position = Camera.main.transform.position.withZ(transform.position.z);
        }

        if (Input.GetKey(KeyCode.Escape)) {
            stuff.SetActive(true);
            timeHeld += Time.deltaTime;
            if (timeHeld >= TimeToHold) {
                stuff.SetActive(false);
                SceneManager.LoadScene("MainMenu");
            }
            shaker.Strength = initialShake * (timeHeld / TimeToHold);
        } else {
            stuff.SetActive(false);
            timeHeld = 0;
        }
    }
}
