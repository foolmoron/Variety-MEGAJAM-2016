using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    [Range(0, 1)]
    public float Dirtiness;
    public Player CurrentDirtyPerson;
    Vector3 previousDirtyPersonPosition;
    public AnimationCurve DirtinessByMovement;
    public float CurrentDirtyMovement;

    public Color CleanColor;
    public Color DirtyColor;
    SpriteRenderer sprite;

    void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        // get dirtier as dirty person moves
        {
            if (CurrentDirtyPerson) {
                CurrentDirtyMovement += (CurrentDirtyPerson.transform.position - previousDirtyPersonPosition).magnitude;
                previousDirtyPersonPosition = CurrentDirtyPerson.transform.position;
            }
            Dirtiness = DirtinessByMovement.Evaluate(CurrentDirtyMovement);
        }
        // set color based on dirtiness
        {
            sprite.color = Color.Lerp(CleanColor, DirtyColor, Dirtiness * Dirtiness);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        var player = collision.GetComponent<Player>();
        if (player) {
            CurrentDirtyPerson = player;
            previousDirtyPersonPosition = CurrentDirtyPerson.transform.position;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        var player = collision.GetComponent<Player>();
        if (CurrentDirtyPerson == player) {
            CurrentDirtyPerson = null;
        }
    }
}