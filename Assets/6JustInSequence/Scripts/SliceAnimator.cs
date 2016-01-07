using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliceAnimator : MonoBehaviour {

    // requires 20 slices because whatever
    public Image[] Slices;

    void Start() {
        for (int i = 0; i < Slices.Length; i++) {
            ResetSlice(i);
        }
    }

    public void ResetSlice(int i) {
        var slice = Slices[i];
        slice.transform.localScale = new Vector3(0, 0, 1);
        slice.fillAmount = 0.05f;
        slice.transform.rotation = Quaternion.Euler(0, 0, -45 + 18 * i);
    }

    Coroutine topFlash;
    public void TopFlash() {
        if (topFlash != null) {
            StopCoroutine(topFlash);
        }
        for (int i = 6; i <= 8; i++) {
            ResetSlice(i);
        }
        topFlash = StartCoroutine(FlashAnimation(6, 7, 8, 0.25f, 0.12f));
    }

    Coroutine bottomFlash;
    public void BottomFlash() {
        if (bottomFlash != null) {
            StopCoroutine(bottomFlash);
        }
        for (int i = 16; i <= 18; i++) {
            ResetSlice(i);
        }
        bottomFlash = StartCoroutine(FlashAnimation(16, 17, 18, 0.25f, 0.12f));
    }

    Coroutine rightFlash;
    public void RightFlash() {
        if (rightFlash != null) {
            StopCoroutine(rightFlash);
        }
        for (int i = 1; i <= 3; i++) {
            ResetSlice(i);
        }
        rightFlash = StartCoroutine(FlashAnimation(1, 2, 3, 0.25f, 0.12f));
    }

    Coroutine leftFlash;
    public void LeftFlash() {
        if (leftFlash != null) {
            StopCoroutine(leftFlash);
        }
        for (int i = 11; i <= 13; i++) {
            ResetSlice(i);
        }
        leftFlash = StartCoroutine(FlashAnimation(11, 12, 13, 0.25f, 0.12f));
    }

    IEnumerator FlashAnimation(int first, int middle, int last, float sprayTime, float wipeTime) {
        var t = 0f;
        while (t < sprayTime) {
            t += Time.deltaTime;
            Slices[first].transform.localScale = new Vector3(Interpolate.Ease(Interpolate.EaseType.Linear)(0, 1, t, sprayTime), Interpolate.Ease(Interpolate.EaseType.Linear)(0, 1, t, sprayTime));
            Slices[middle].transform.localScale = new Vector3(Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(0, 1, t, sprayTime), Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(0, 1, t, sprayTime));
            Slices[last].transform.localScale = new Vector3(Interpolate.Ease(Interpolate.EaseType.Linear)(0, 1, t, sprayTime), Interpolate.Ease(Interpolate.EaseType.Linear)(0, 1, t, sprayTime));
            yield return new WaitForEndOfFrame();
        }
        t = 0f;
        while (t < wipeTime) {
            t += Time.deltaTime;
            Slices[first].fillAmount = Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(0.05f, -0.05f, t, wipeTime);
            Slices[middle].fillAmount = Interpolate.Ease(Interpolate.EaseType.EaseInSine)(0.05f, -0.05f, t, wipeTime);
            Slices[last].fillAmount = Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(0.05f, -0.05f, t, wipeTime);
            Slices[first].transform.rotation = Quaternion.Euler(0, 0, Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(-45 + 18 * first, 9, t, wipeTime));
            Slices[middle].transform.rotation = Quaternion.Euler(0, 0, Interpolate.Ease(Interpolate.EaseType.EaseInSine)(-45 + 18 * middle, 9, t, wipeTime));
            Slices[last].transform.rotation = Quaternion.Euler(0, 0, Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(-45 + 18 * last, 9, t, wipeTime));
            yield return new WaitForEndOfFrame();
        }
    }

    void Update() {
    }
}