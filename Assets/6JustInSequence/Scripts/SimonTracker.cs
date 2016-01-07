using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimonTracker : MonoBehaviour {

    public GhostPulser TopButton;
    public GhostPulser BottomButton;
    public GhostPulser RightButton;
    public GhostPulser LeftButton;

    public int CurrentSequenceIndex;

    [Range(0, 4)]
    public float SwitchDelay = 2.5f;
    bool shouldSwitch;
    float switchTime;

    TurnTracker turnTracker;
    SliceAnimator sliceAnimator;

    void Start() {
        turnTracker = FindObjectOfType<TurnTracker>();
        sliceAnimator = FindObjectOfType<SliceAnimator>();
    }
    
    void Update() {
        // get input 
        {
            if (turnTracker.PlayerTurn && !shouldSwitch) {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                    TopButton.Pulse();
                    sliceAnimator.TopFlash();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Up") {
                        CurrentSequenceIndex = -1;
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    BottomButton.Pulse();
                    sliceAnimator.BottomFlash();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Down") {
                        CurrentSequenceIndex = -1;
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                    RightButton.Pulse();
                    sliceAnimator.RightFlash();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Right") {
                        CurrentSequenceIndex = -1;
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    LeftButton.Pulse();
                    sliceAnimator.LeftFlash();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Left") {
                        CurrentSequenceIndex = -1;
                    } else {
                        CurrentSequenceIndex++;
                    }
                }
                if (CurrentSequenceIndex == turnTracker.CurrentSequence.Count) {
                    sliceAnimator.Victory();
                    turnTracker.IncreaseLevel();
                    shouldSwitch = true;
                    switchTime = 0;
                    CurrentSequenceIndex = 0;
                } else if (CurrentSequenceIndex < 0) {
                    sliceAnimator.Error();
                    turnTracker.Reset();
                    shouldSwitch = true;
                    switchTime = 0;
                    CurrentSequenceIndex = 0;
                }
            }
        }
        // do victory after delay
        {
            if (shouldSwitch) {
                switchTime += Time.deltaTime;
                if (switchTime >= SwitchDelay) {
                    shouldSwitch = false;
                    turnTracker.PlayerTurn = false;
                }
            }
        }
    }
}