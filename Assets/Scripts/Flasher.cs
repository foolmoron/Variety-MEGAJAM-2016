using UnityEngine;
using System.Collections;

public class Flasher : MonoBehaviour {

    [Range(0, 3)]
    public float OnTime;
    [Range(0, 3)]
    public float OffTime;
    bool currentlyOn = true;
    float time;

    new Renderer renderer;

    void Start() {
        renderer = GetComponent<Renderer>();
    }
    
    void Update () {
        time += Time.deltaTime;
        if ((currentlyOn && time >= OnTime) || (!currentlyOn && time >= OffTime)) {
            currentlyOn = !currentlyOn;
            renderer.enabled = currentlyOn;
            time = 0;
        }
    }
}
