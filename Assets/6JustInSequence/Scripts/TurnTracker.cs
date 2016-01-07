using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurnTracker : MonoBehaviour {

    public bool PlayerTurn;

    public Image MiddleSprite;

    public Color PlayerColor = Color.white;
    public Color SimonColor = Color.black;
    [Range(0, 0.5f)]
    public float ColorSpeed = 0.25f;
    
    public int Level;

    public List<string> CurrentSequence = new List<string>(100);

    [Range(0, 2)]
    public float MinButtonInterval = 0.25f;
    [Range(0, 2)]
    public float MaxButtonInterval = 1f;
    float buttonTime = -2f;

    [Range(0, 2)]
    public float SwitchDelay = 1f;
    bool shouldSwitch;
    float switchTime;

    SimonTracker simonTracker;
    SliceAnimator sliceAnimator;

    TextMesh listenText;
    TextMesh repeatText;

    void Start() {
        simonTracker = FindObjectOfType<SimonTracker>();
        sliceAnimator = FindObjectOfType<SliceAnimator>();
        listenText = transform.FindChild("ListenText").GetComponent<TextMesh>();
        repeatText = transform.FindChild("RepeatText").GetComponent<TextMesh>();
    }

    public void IncreaseLevel() {
        Level++;
        CurrentSequence.Clear();
        buttonTime = -1f;
        switchTime = 0;
    }

    void Update() {
        // change colors based on turn
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, PlayerTurn ? PlayerColor : SimonColor, ColorSpeed);
            MiddleSprite.color = Color.Lerp(MiddleSprite.color, PlayerTurn ? PlayerColor : SimonColor, ColorSpeed);
        }
        // text based on turn and level
        {
            listenText.gameObject.SetActive(Level < 2 && !PlayerTurn);
            repeatText.gameObject.SetActive(Level < 2 && PlayerTurn);
        }
        // press buttons if it's simon's turn
        {
            if (!PlayerTurn && !shouldSwitch) {
                buttonTime += Time.deltaTime;
                if (buttonTime >= Mathf.Lerp(MaxButtonInterval, MinButtonInterval, (float)Level/10)) {
                    switch (Random.Range(0, 4)) {
                        case 0:
                            CurrentSequence.Add("Up");
                            simonTracker.TopButton.Pulse();
                            sliceAnimator.TopFlash();
                            break;
                        case 1:
                            CurrentSequence.Add("Down");
                            simonTracker.BottomButton.Pulse();
                            sliceAnimator.BottomFlash();
                            break;
                        case 2:
                            CurrentSequence.Add("Right");
                            simonTracker.RightButton.Pulse();
                            sliceAnimator.RightFlash();
                            break;
                        case 3:
                            CurrentSequence.Add("Left");
                            simonTracker.LeftButton.Pulse();
                            sliceAnimator.LeftFlash();
                            break;
                    }
                    buttonTime = 0;
                }
                if (CurrentSequence.Count >= (Level / 2 + 1)) {
                    shouldSwitch = true;
                }
            }
        }
        // switch after delay
        {
            if (shouldSwitch) {
                switchTime += Time.deltaTime;
                if (switchTime >= SwitchDelay) {
                    PlayerTurn = true;
                    shouldSwitch = false;
                    switchTime = 0;
                }
            }
        }
    }
}