using UnityEngine;
using System.Collections;

public class SimonTracker : MonoBehaviour {

    public GhostPulser TopButton;
    public GhostPulser BottomButton;
    public GhostPulser RightButton;
    public GhostPulser LeftButton;

    void Start() {
    }

    void Update() {
        // get input 
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                TopButton.Pulse();
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                BottomButton.Pulse();
            } else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                RightButton.Pulse();
            } else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                LeftButton.Pulse();
            }
        }
    }
}