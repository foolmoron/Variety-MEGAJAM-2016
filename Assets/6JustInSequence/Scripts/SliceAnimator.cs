using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliceAnimator : MonoBehaviour {

    // requires 20 slices because whatever
    public Image[] Slices;
    Color[] originalColors;

    void Start() {
        originalColors = new Color[Slices.Length];
        for (int i = 0; i < Slices.Length; i++) {
            originalColors[i] = Slices[i].color;
        }
        for (int i = 0; i < Slices.Length; i++) {
            ResetSlice(i);
        }
    }

    public void ResetSlice(int i) {
        var slice = Slices[i];
        slice.color = originalColors[i];
        slice.transform.localScale = new Vector3(0, 0, 1);
        slice.transform.rotation = Quaternion.Euler(0, 0, -45 + 18 * i);
        slice.fillAmount = 0.05f;
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

    Coroutine victory;
    public void Victory() {
        StopAllCoroutines();
        for (int i = 0; i < Slices.Length; i++) {
            ResetSlice(i);
        }
        victory = StartCoroutine(RadialColors(0.4f, 1.2f, 0.4f, false, 1f));
    }

    Coroutine error;
    public void Error() {
        StopAllCoroutines();
        for (int i = 0; i < Slices.Length; i++) {
            ResetSlice(i);
        }
        error = StartCoroutine(RadialColors(0.3f, 0.4f, 1.0f, true, 1f));
    }

    IEnumerator RadialColors(float sprayMaxTime, float rotateTime, float wipeMaxTime, bool red, float hueSpeed) {
        var sprayTimes = new float[Slices.Length];
        for (int i = 0; i < sprayTimes.Length; i++) {
            sprayTimes[i] = Mathf.Lerp(sprayMaxTime / 4, sprayMaxTime, Random.value);
        }
        var wipeTimes = new float[Slices.Length];
        for (int i = 0; i < wipeTimes.Length; i++) {
            wipeTimes[i] = Mathf.Lerp(wipeMaxTime / 4, wipeMaxTime, Random.value);
        }

        var overallTime = 0f;
        while (overallTime < (sprayMaxTime + rotateTime + wipeMaxTime)) {
            overallTime += Time.deltaTime;
            if (overallTime > 0 && overallTime < sprayMaxTime) {
                var t = overallTime - 0;
                for (int i = 0; i < Slices.Length; i++) {
                    Slices[i].transform.localScale = new Vector3(Interpolate.Ease(Interpolate.EaseType.Linear)(0, 1, t, sprayTimes[i]), Interpolate.Ease(Interpolate.EaseType.Linear)(0, 1, t, sprayTimes[i]));
                }
            } else if (overallTime >= sprayMaxTime && overallTime < (sprayMaxTime + rotateTime)) {
                var t = overallTime - sprayMaxTime;
            } else if (overallTime >= (sprayMaxTime + rotateTime) && overallTime < (sprayMaxTime + rotateTime + wipeMaxTime)) {
                var t = overallTime - (sprayMaxTime + rotateTime);
                for (int i = 0; i < Slices.Length; i++) {
                    Slices[i].fillAmount = Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(0.05f, -0.05f, t, wipeTimes[i]);
                    Slices[i].transform.rotation = Quaternion.Euler(0, 0, Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(-45 + 18 * i, 9, t, wipeTimes[i]));
                }
            }

            for (int i = 0; i < Slices.Length; i++) {
                var slice = Slices[i];
                var lerp = ((float)i / Slices.Length + (overallTime * hueSpeed)) % 1;
                slice.color = new HSBColor(red ? (0.96f + Mathf.Sin(lerp * 6 * Mathf.PI) * 0.04f) % 1 : lerp, 1, 1, 1).ToColor();
            }
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < Slices.Length; i++) {
            ResetSlice(i);
        }
    }
}