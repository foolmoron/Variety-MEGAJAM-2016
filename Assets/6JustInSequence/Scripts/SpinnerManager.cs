using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpinnerManager : MonoBehaviour {
    
    [Range(1, 200)]
    public int Spinners = 10;
    public GameObject SpinnerPrefab;
    [Range(0, 360)]
    public float RotationStep;
    [Range(-45, 45)]
    public float RotationStepVelocity;
    [Range(0, 1)]
    public float ScaleShake;

    public bool Play;

    SliceAnimator[] SliceAnimators;

    void Start() {
        SliceAnimators = new SliceAnimator[Spinners];
        for (int i = 0; i < SliceAnimators.Length; i++) {
            var sliceAnimator = Instantiate(SpinnerPrefab).GetComponent<SliceAnimator>();
            sliceAnimator.transform.localPosition = sliceAnimator.transform.localPosition.withZ(-i*0.001f);

            SliceAnimators[i] = sliceAnimator;
            sliceAnimator.transform.localScale = new Vector3(1.5f / Spinners * (Spinners - i), 1.5f / Spinners * (Spinners - i), 1);
        }
        RotationStep = 360f / Spinners;
    }

    public void Update() {
        RotationStep = (RotationStep + RotationStepVelocity * Time.deltaTime) % 360;
        for (int i = 0; i < SliceAnimators.Length; i++) {
            var sliceAnimator = SliceAnimators[i];
            sliceAnimator.transform.localRotation = Quaternion.Euler(0, 0, RotationStep * i);
            sliceAnimator.Play = Play;
            sliceAnimator.ScaleShake = ScaleShake;
        }
    }
}