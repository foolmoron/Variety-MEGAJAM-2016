using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Staticer : MonoBehaviour {

    [Range(0, 1)]
    public float Amount;
    [Range(0, 30)]
    public int OnFrames;
    public Image Static1;
    public Image Static2;

    ScaryItem[] items;

    void Start() {
        items = FindObjectsOfType<ScaryItem>();
    }

    void Update() {
        if (OnFrames > 0) {
            Static1.color = Color.white;
            Static2.color = Color.white;
            OnFrames--;
            for (int i = 0; i < items.Length; i++) {
                if (items[i]) {
                   items[i].gameObject.SetActive(false);   
                }
            }
        } else {
            Static1.color = Color.white.withAlpha(Amount);
            Static2.color = Color.white.withAlpha(Amount);
            for (int i = 0; i < items.Length; i++) {
                if (items[i]) {
                    items[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
