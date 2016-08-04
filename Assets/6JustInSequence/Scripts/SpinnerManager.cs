using UnityEngine;
using System.Collections;
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
    [Range(0, 1)]
    public float ScaleShake;

    public bool Play;

    SliceAnimator[] SliceAnimators;

    public GameObject CreditsObject;
    [Range(0, 5)]
    public float CreditsHoldTime = 2;
    float creditsHoldTime;
    public bool Credits;

    void Start() {
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
                        lerp /= 1 + zeroRange;
                    } else {
                        lerp *= 1 + zeroRange;
                    }
                    RotationStep = Mathf.Pow((lerp - 0.5f) * 2, exp) * Mathf.Sign(lerp - 0.5f) * 180;
                } else if (screenPos.x <= horizontalSliderRegion && screenPos.y <= 0.5f) { // rot velocity horizontal slider
                    var lerp = screenPos.x / horizontalSliderRegion;
                    if (Mathf.Abs(lerp - 0.5f) <= zeroRange / 2) {
                        lerp = 0.5f;
                    } else if (lerp >= 0.5f) {
                        lerp /= 1 + zeroRange;
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
        // set slices
        {
            RotationStep = (RotationStep + RotationStepVelocity * Time.deltaTime) % 360;
            for (int i = 0; i < SliceAnimators.Length; i++) {
                var sliceAnimator = SliceAnimators[i];
                sliceAnimator.transform.localRotation = Quaternion.Euler(0, 0, RotationStep * i);
                sliceAnimator.Play = Play;
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