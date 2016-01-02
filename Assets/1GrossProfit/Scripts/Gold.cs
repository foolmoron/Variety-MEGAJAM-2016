using UnityEngine;
using System.Collections;

public class Gold : MonoBehaviour {

    [Range(0, 5)]
    public int GoldLeft = 3;

    int originalGold;
    Vector3 originalScale;

    void Start() {
        originalGold = GoldLeft;
        originalScale = transform.localScale;
    }

    public void ResetGoldLeft() {
        GoldLeft = originalGold;
    }

    void Update() {
        var scaleInterp = ((float)GoldLeft / originalGold) * 0.8f + 0.2f;
        transform.localScale = originalScale * scaleInterp;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        var goldReceiver = collision.GetComponent<GoldReceiver>();
        if (goldReceiver && goldReceiver.GoldAmount == 0) {
            GoldLeft--;
            if (GoldLeft == 0) {
                Destroy(gameObject);
            }
            goldReceiver.OnReceiveGold();
        }
    }
}