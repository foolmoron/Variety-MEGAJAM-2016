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


    public AudioClip UpSound;
    public AudioClip DownSound;
    public AudioClip RightSound;
    public AudioClip LeftSound;
    public AudioClip WinSound;
    public AudioClip LoseSound;
    AudioSource audioSource;

    void Start() {
        turnTracker = FindObjectOfType<TurnTracker>();
        sliceAnimator = FindObjectOfType<SliceAnimator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void AnimateUp() {
        TopButton.Pulse();
        sliceAnimator.TopFlash();
        audioSource.PlayOneShot(UpSound);
    }
    public void AnimateDown() {
        BottomButton.Pulse();
        sliceAnimator.BottomFlash();
        audioSource.PlayOneShot(DownSound);
    }
    public void AnimateRight() {
        RightButton.Pulse();
        sliceAnimator.RightFlash();
        audioSource.PlayOneShot(RightSound);
    }
    public void AnimateLeft() {
        LeftButton.Pulse();
        sliceAnimator.LeftFlash();
        audioSource.PlayOneShot(LeftSound);
    }

    void Update() {
        // get input 
        {
            if (turnTracker.PlayerTurn && !shouldSwitch) {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                    AnimateUp();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Up") {
                        CurrentSequenceIndex = -1;
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    AnimateDown();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Down") {
                        CurrentSequenceIndex = -1;
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                    AnimateRight();
                    if (turnTracker.CurrentSequence[CurrentSequenceIndex] != "Right") {
                        CurrentSequenceIndex = -1;
                    } else {
                        CurrentSequenceIndex++;
                    }
                } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    AnimateLeft();
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
                    audioSource.PlayOneShot(WinSound);
                } else if (CurrentSequenceIndex < 0) {
                    sliceAnimator.Error();
                    turnTracker.Reset();
                    shouldSwitch = true;
                    switchTime = 0;
                    CurrentSequenceIndex = 0;
                    audioSource.PlayOneShot(LoseSound);
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