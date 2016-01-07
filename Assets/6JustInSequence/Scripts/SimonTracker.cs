using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimonTracker : MonoBehaviour {

    public GhostPulser TopButton;
    public GhostPulser BottomButton;
    public GhostPulser RightButton;
    public GhostPulser LeftButton;

    public int CurrentSequenceIndex;

    [Range(0, 2)]
    public float VictoryDelay = 1f;
    bool shouldVictory;
    float victoryTime;

    TurnTracker turnTracker;
    SliceAnimator sliceAnimator;

    void Start() {
        turnTracker = FindObjectOfType<TurnTracker>();
        sliceAnimator = FindObjectOfType<SliceAnimator>();
    }

    public void Error() {
        CurrentSequenceIndex = 0;
    }

    void Update() {
        // get input 
        {
            if (turnTracker.PlayerTurn && !shouldVictory) {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                    TopButton.Pulse();
                    sliceAnimator.TopFlash();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Up") {
                        Error();
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    BottomButton.Pulse();
                    sliceAnimator.BottomFlash();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Down") {
                        Error();
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                    RightButton.Pulse();
                    sliceAnimator.RightFlash();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Right") {
                        Error();
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    LeftButton.Pulse();
                    sliceAnimator.LeftFlash();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Left") {
                        Error();
                    } else {
                        CurrentSequenceIndex++;
                    }
                }
                if (CurrentSequenceIndex == turnTracker.CurrentSequence.Count) {
                    sliceAnimator.Victory();
                    turnTracker.IncreaseLevel();
                    shouldVictory = true;
                    victoryTime = 0;
                    CurrentSequenceIndex = 0;
                }
            }
        }
        // do victory after delay
        {
            if (shouldVictory) {
                victoryTime += Time.deltaTime;
                if (victoryTime >= VictoryDelay) {
                    shouldVictory = false;
                    turnTracker.PlayerTurn = false;
                }
            }
        }
    }
}