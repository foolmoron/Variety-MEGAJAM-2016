using UnityEngine;
using System.Collections;

public class GoldReceiver : MonoBehaviour {

    public GameObject GoldPrefab;
    public Vector3 GoldOffset = new Vector3(0f, 0.1f, 0f);
    public Vector3 GoldOriginalScale;
    GameObject gold;

    public float TimeSinceNoGold;
    [Range(0, 100)]
    public float TimeToGoldThought = 10;
    GameObject goldThought;

    [Range(0, 1)]
    public float GoldAmount;
    [Range(0, 10)]
    public float GoldConsumedPerMinute;

    void Start() {
        // initialize gold
        {
            gold = Instantiate(GoldPrefab);
            gold.transform.parent = transform;
            gold.transform.localPosition = GoldOffset;
            gold.transform.localRotation = Quaternion.identity;
            GoldOriginalScale = gold.transform.localScale;
        }
        // initialize gold thought bubble
        {
            goldThought = transform.FindChild("GoldThought").gameObject;
            TimeSinceNoGold = Random.value * TimeToGoldThought * 0.8f; // stagger the initial thought bubbles
        }
    }

    void Update() {
        // consume gold
        {
            GoldAmount = Mathf.MoveTowards(GoldAmount, 0, (GoldConsumedPerMinute / 60) * Time.deltaTime);
        }
        // scale gold based on amount left
        {
            if (GoldAmount > 0) {
                gold.SetActive(true);
                var scaleInterp = GoldAmount * 0.8f + 0.2f;
                gold.transform.localScale = GoldOriginalScale * scaleInterp;
            } else {
                gold.SetActive(false);
            }
        }
        // count how long we've been without gold
        {
            if (GoldAmount == 0) {
                TimeSinceNoGold += Time.deltaTime;
            } else {
                TimeSinceNoGold = 0;
            }
        }
        // show gold thought bubble based on how much time has passed since no gold
        {
            goldThought.SetActive(TimeSinceNoGold > TimeToGoldThought);
        }
    }

    public void OnReceiveGold() {
        GoldAmount = 1;
    }
}