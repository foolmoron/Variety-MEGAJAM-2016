using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReloadWhenKey : MonoBehaviour {

    public KeyCode Key = KeyCode.Escape;
    
    void Update() {
        if (Input.GetKeyDown(Key)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
