using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [Range(0, 10)]
    public float Speed = 4;
    [Range(0, 1)]
    public float Encumbrance;

    SpriteRenderer mainSprite;
    public Sprite RegularSprite;
    public Sprite WaterSprite;

    void Start() {
        mainSprite = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
    }

    void Update() {
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
        {
            var movement = Speed * (1 - Encumbrance) * Time.deltaTime * input;
            transform.position += movement.to3();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        var water = collision.GetComponent<Water>();
        if (water) {
            mainSprite.sprite = WaterSprite;
            Encumbrance = water.Encumbrance;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        var water = collision.GetComponent<Water>();
        if (water) {
            mainSprite.sprite = RegularSprite;
            Encumbrance = 0;
        }
    }
}