using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [Range(0, 10)]
    public float Speed = 4;
    [Range(0, 1)]
    public float Encumbrance;
    [Range(0, 1)]
    public float GoldEncumbrance = 0.66f;

    public Vector2 Bounds;

    public Vector3 GoldOffset = new Vector3(0, 0.44f, 0);
    public Gold CurrentGold;

    Water water;
    public bool InWater;

    public AnimationCurve MoveBobbing;
    public float MoveAnimationScale;
    float animationPosition;

    SpriteRenderer mainSprite;
    public Sprite RegularSprite;
    public Sprite WaterSprite;

    public float RotationSoundTrigger = 0.35f;
    bool soundTriggered = false;
    public AudioClip RegularStep;
    public AudioClip WaterStep;

    public AudioClip GoldSound;

    void Start() {
        mainSprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        water = FindObjectOfType<Water>();
    }

    void Update() {
        // encumbrance/sprite based on water
        {
            if (InWater) {
                Encumbrance = water.Dirtiness;
                mainSprite.sprite = WaterSprite;
            } else {
                Encumbrance = 0;
                mainSprite.sprite = RegularSprite;
            }
        }
        var input = Vector2.zero;
        // get input 
        {
            // mouse click input first
            if (Input.GetMouseButton(0)) {
                var vectorToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).to2();
                if (vectorToMouse.magnitude > 0.2f) {
                    input = vectorToMouse.normalized;
                }
            } else {
                // then keyboard wasd/arrows input
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    input.y = 1;
                } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    input.y = -1;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    input.x = 1;
                } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    input.x = -1;
                }
                input = input.normalized;
            }
        }
        // move based on input
        Vector2 movement;
        {
            movement = Speed * (1 - Encumbrance) * (CurrentGold ? GoldEncumbrance : 1f) * Time.deltaTime * input;
            var newPos = transform.position + movement.to3();
            newPos.x = Mathf.Clamp(newPos.x, -Bounds.x / 2, Bounds.x / 2);
            newPos.y = Mathf.Clamp(newPos.y, -Bounds.y / 2, Bounds.y / 2);
            transform.position = newPos;
        }
        // animate based on movement
        {
            if (movement != Vector2.zero) {
                animationPosition += movement.magnitude * MoveAnimationScale;
                mainSprite.transform.localRotation = Quaternion.Euler(0, 0, MoveBobbing.Evaluate(animationPosition) * 360f);
            } else {
                mainSprite.transform.localRotation = Quaternion.identity;
            }
        }
        // trigger sound based on animation
        {
            var angle = Mathf.Min(Mathf.Abs(mainSprite.transform.localRotation.eulerAngles.z), Mathf.Abs(mainSprite.transform.localRotation.eulerAngles.z - 360));
            if (!soundTriggered && angle >= RotationSoundTrigger) {
                soundTriggered = true;
                AudioSource.PlayClipAtPoint(InWater ? WaterStep : RegularStep, Vector3.zero);
            } else if (angle < RotationSoundTrigger) {
                soundTriggered = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        var water = collision.GetComponent<Water>();
        if (water) {
            InWater = true;
            AudioSource.PlayClipAtPoint(WaterStep, Vector3.zero);
        }
        var goldSource = collision.GetComponent<GoldSource>();
        if (goldSource) {
            if (!CurrentGold) {
                CurrentGold = Instantiate(goldSource.GoldPrefab).GetComponent<Gold>();
                CurrentGold.transform.parent = transform;
                CurrentGold.transform.localPosition = GoldOffset;
                CurrentGold.transform.localRotation = Quaternion.identity;
            } else {
                CurrentGold.ResetGoldLeft();
            }
            AudioSource.PlayClipAtPoint(GoldSound, Vector3.zero);
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        var water = collision.GetComponent<Water>();
        if (water) {
            InWater = false;
            AudioSource.PlayClipAtPoint(RegularStep, Vector3.zero);
        }
    }
}