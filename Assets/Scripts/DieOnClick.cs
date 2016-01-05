using UnityEngine;
using System.Collections;

public class DieOnClick : MonoBehaviour {
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Destroy(gameObject);
        }
    }
}
