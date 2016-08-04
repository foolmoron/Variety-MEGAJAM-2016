using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliceAnimator : MonoBehaviour {

    // requires 20 slices because whatever
    public SliceMesh[] Slices;
    public Color InitialColor;
    Color[] originalColors;

    public bool Play;
    bool prevPlay;
    [SplitRange(0, 1.5f, 10)]
    public float TimeScale = 1;
    [Range(0, 1)]
    public float ScaleShake = 0;

    void Awake() {
        // set colors
        var hsb = HSBColor.FromColor(InitialColor);
        for (int i = 0; i < Slices.Length; i++) {
            var slice = Slices[i];
            var lerp = (float)i / Slices.Length;
            slice.Color = new HSBColor((hsb.h + lerp) % 1, hsb.s, hsb.b, hsb.a).ToColor();
        }
    }

    void Start() {
        originalColors = new Color[Slices.Length];
        for (int i = 0; i < Slices.Length; i++) {
            originalColors[i] = Slices[i].Color;
            ResetSlice(i);
        }
    }

    public void Update() {
        if (Play && !prevPlay) {
            for (int i = 0; i < Slices.Length; i++) {
                ResetSlice(i);
            }
            victory = StartCoroutine(RadialColors(0.5f, float.PositiveInfinity, 0.4f, false, 1f));
        } else if (!Play && prevPlay) {
            if (victory != null) {
                StopCoroutine(victory);
            }
        }
        prevPlay = Play;
    }

    public void ResetSlice(int i) {
        var slice = Slices[i];
        slice.Color = originalColors[i];
        slice.transform.localScale = new Vector3(0, 0, 1);
        slice.transform.localRotation = Quaternion.Euler(0, 0, -45 + 18 * i);
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
            Slices[first].transform.localRotation = Quaternion.Euler(0, 0, Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(-45 + 18 * first, 9, t, wipeTime));
            Slices[middle].transform.localRotation = Quaternion.Euler(0, 0, Interpolate.Ease(Interpolate.EaseType.EaseInSine)(-45 + 18 * middle, 9, t, wipeTime));
            Slices[last].transform.localRotation = Quaternion.Euler(0, 0, Interpolate.Ease(Interpolate.EaseType.EaseOutSine)(-45 + 18 * last, 9, t, wipeTime));
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
        var sprayScales = new float[Slices.Length];
        var prevSprayScales = new float[Slices.Length];
        for (int i = 0; i < sprayScales.Length; i++) {
            prevSprayScales[i] = 0;
            sprayScales[i] = 1;
        }

        var t = 0f;
        var ts = 0f;
        while (t < (sprayMaxTime + rotateTime + wipeMaxTime)) {
            t += Time.deltaTime * TimeScale;
            ts += Time.deltaTime * TimeScale;
            if (ts >= sprayMaxTime) {
                ts = 0;
                for (int i = 0; i < sprayScales.Length; i++) {
                    prevSprayScales[i] = sprayScales[i];
                    sprayScales[i] = Random.value * ScaleShake + (1 - ScaleShake);
                }
            }
            for (int i = 0; i < Slices.Length; i++) {
                var slice = Slices[i];
                
                var scale = Mathf.Lerp(prevSprayScales[i], sprayScales[i], ts/sprayMaxTime);
                slice.transform.localScale = new Vector3(scale, scale);

                var lerp = ((float)i / Slices.Length + (t * hueSpeed)) % 1;
                slice.Color = new HSBColor(red ? (0.96f + Mathf.Sin(lerp * 6 * Mathf.PI) * 0.04f) % 1 : lerp, 1, 1, 1).ToColor();
            }
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < Slices.Length; i++) {
            ResetSlice(i);
        }
    }
}