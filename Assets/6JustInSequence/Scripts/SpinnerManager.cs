using System;
using UnityEngine;
using System.Collections;
using MidiJack;
using UnityEngine.UI;

public class SpinnerManager : MonoBehaviour {
    
    [Range(1, 200)]
    public int Spinners = 10;
    public GameObject SpinnerPrefab;
    public float MaxScale = 1f;
    [Range(-180, 180)]
    public float RotationStep;
    [Range(-45, 45)]
    public float RotationStepVelocity;
    [Range(-5, 5)]
    public float HueSpeed;
    [Range(0, 1)]
    public float ScaleShake;

    public static bool USE_MIDI = true;

    public bool Play;

    SliceAnimator[] SliceAnimators;

    public GameObject CreditsObject;
    [Range(0, 5)]
    public float CreditsHoldTime = 2;
    float creditsHoldTime;
    public bool Credits;

    FullscreenEffect effect;

    void Start() {
        effect = FindObjectOfType<FullscreenEffect>();
        SliceAnimators = new SliceAnimator[Spinners];
        for (int i = 0; i < SliceAnimators.Length; i++) {
            var sliceAnimator = Instantiate(SpinnerPrefab).GetComponent<SliceAnimator>();
            sliceAnimator.transform.localPosition = sliceAnimator.transform.localPosition.withZ(-i*0.001f);

            SliceAnimators[i] = sliceAnimator;
            sliceAnimator.transform.localScale = new Vector3(MaxScale / Spinners * (Spinners - i), MaxScale / Spinners * (Spinners - i), 1);
        }
        RotationStep = 360f / Spinners;
    }

    public void Update() {
        // controls
        {
            var horizontalSliderRegion = 0.7f;
            var verticalSliderRegion = 0.9f;
            var zeroRange = 0.1f;
            var exp = 2f;
            if (Input.GetMouseButton(0)) {
                var screenPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
                if (screenPos.x <= horizontalSliderRegion && screenPos.y >= 0.5f) { // rot step horizontal slider
                    var lerp = screenPos.x / horizontalSliderRegion;
                    if (Mathf.Abs(lerp - 0.5f) <= zeroRange / 2) {
                        lerp = 0.5f;
                    } else if (lerp >= 0.5f) {
                        lerp /= 1 + zeroRange * (1 - (lerp - 0.5f) * 2);
                    } else {
                        lerp *= 1 + zeroRange;
                    }
                    RotationStep = Mathf.Pow((lerp - 0.5f) * 2, exp) * Mathf.Sign(lerp - 0.5f) * 180;
                } else if (screenPos.x <= horizontalSliderRegion && screenPos.y <= 0.5f) { // rot velocity horizontal slider
                    var lerp = screenPos.x / horizontalSliderRegion;
                    if (Mathf.Abs(lerp - 0.5f) <= zeroRange / 2) {
                        lerp = 0.5f;
                    } else if (lerp >= 0.5f) {
                        lerp /= 1 + zeroRange*(1 - (lerp - 0.5f)*2);
                    } else {
                        lerp *= 1 + zeroRange;
                    }
                    RotationStepVelocity = Mathf.Pow((lerp - 0.5f) * 2, exp) * Mathf.Sign(lerp - 0.5f) * 45;
                } else if (screenPos.x >= verticalSliderRegion) { // shake vertical slider
                    var lerp = screenPos.y;
                    if (lerp <= zeroRange) {
                        lerp = 0f;
                    } else {
                        lerp = (lerp - zeroRange) / (1 - zeroRange);
                    }
                    ScaleShake = Mathf.Pow(lerp, exp);
                }
            }
        }
        // midi
        {
            if (MidiMaster.GetKnob(0x2D) > 0) {
                USE_MIDI = true;
            } else if (MidiMaster.GetKnob(0x2E) > 0) {
                USE_MIDI = false;
            }
            if (USE_MIDI) {
                RotationStepVelocity = -(Mathf.Sqrt(MidiMaster.GetKnob(0x0E)) * 2 - 1) * 90;
                if (MidiMaster.GetKnob(0x18) > 0f) {
                    RotationStep = -(MidiMaster.GetKnob(0x0F) * 2 - 1) * MidiMaster.GetKnob(0x04) * 180;
                    RotationStepVelocity = 0;
                }
                HueSpeed = -(Mathf.Sqrt(MidiMaster.GetKnob(0x10)) * 2 - 1) * 5;
                effect.Effect.SetFloat("_Saturation", MidiMaster.GetKnob(0x06));
            }
            ScaleShake = MidiMaster.GetKnob(0x05) * MidiMaster.GetKnob(0x05);
            Time.timeScale = (MidiMaster.GetKnob(0x03) * 2) * (MidiMaster.GetKnob(0x03) * 2);
        }
        // set slices
        {
            RotationStep = (RotationStep + RotationStepVelocity * Time.deltaTime) % 360;
            for (int i = 0; i < SliceAnimators.Length; i++) {
                var sliceAnimator = SliceAnimators[i];
                sliceAnimator.transform.localRotation = Quaternion.Euler(0, 0, RotationStep * i);
                sliceAnimator.Play = Play;
                sliceAnimator.HueSpeed = HueSpeed;
                sliceAnimator.ScaleShake = ScaleShake;
            }
        }
        // credits
        {
            var screenPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            if (!Credits) {
                if (Input.GetMouseButton(0) && Mathf.Abs(screenPos.x - 0.5f) <= 0.12f && Mathf.Abs(screenPos.y - 0.5f) <= 0.12f) {
                    creditsHoldTime += Time.deltaTime;
                } else {
                    creditsHoldTime = 0;
                }
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    creditsHoldTime = 0;
                }
            }
            Credits = creditsHoldTime >= CreditsHoldTime;
            CreditsObject.SetActive(Credits);
        }
    }
}