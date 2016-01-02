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
    new Animation animation;

    public float JumpAmount = 0.2f;
    public float JumpSpeed = 3f;
    Vector3 originalPosition;
    bool jumpingUp = true;

    [Range(0, 1)]
    public float GoldAmount;
    [Range(0, 10)]
    public float GoldConsumedPerMinute;

    void Start() {
        animation = GetComponent<Animation>();
        originalPosition = transform.position;
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
            if (TimeSinceNoGold > TimeToGoldThought) {
                animation.Play();
            } else {
                animation["GoldDeprived"].enabled = true;
                animation.Sample();
                animation.Stop();
            }
        }
        // jump when happy
        {
            var jumpAmount = JumpAmount * (1 - Mathf.Clamp01(TimeSinceNoGold / (TimeToGoldThought * 0.9f)));
            var jumpSpeed = JumpSpeed * (1 - Mathf.Clamp01(TimeSinceNoGold / (TimeToGoldThought * 0.9f)));
            if (jumpingUp) {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition + new Vector3(0, jumpAmount), jumpSpeed * Time.deltaTime);
                if (transform.position == (originalPosition + new Vector3(0, jumpAmount))) {
                    jumpingUp = false;
                }
            } else {
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, jumpSpeed * Time.deltaTime);
                if (transform.position == originalPosition) {
                    jumpingUp = true;
                }
            }
        }
    }

    public void OnReceiveGold() {
        GoldAmount = 1;
    }
}