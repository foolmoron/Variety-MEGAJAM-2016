using UnityEngine;
using System.Collections;

public class Player7 : MonoBehaviour {

    public GameObject OffOnInput;

    [Range(0, 30)]
    public float JumpForce = 8;
    [Range(0, 30)]
    public float Speed = 5;

    [Range(0, 1080)]
    public float MinRandomRotation = 360;
    [Range(0, 1080)]
    public float MaxRandomRotation = 900;

    public KeyCode UpKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;

    new Rigidbody2D rigidbody;

    ScoreTracker7 scoreTracker;

    public AudioClip JumpSound;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        scoreTracker = FindObjectOfType<ScoreTracker7>();
    }

    void Update() {
        if (scoreTracker.GameOver) {
            return;
        }

        var velocity = rigidbody.velocity;
        if (Input.GetKeyDown(UpKey)) {
            velocity.y += JumpForce;
            rigidbody.angularVelocity = Mathf.Sign(Random.value - 0.5f) * Mathf.Lerp(MinRandomRotation, MaxRandomRotation, Random.value);
            OffOnInput.SetActive(false);
            AudioSource.PlayClipAtPoint(JumpSound, Vector3.zero);
        }
        if (Input.GetKey(LeftKey)) {
            velocity.x = -Speed;
            OffOnInput.SetActive(false);
        } else if (Input.GetKey(RightKey)) {
            velocity.x = Speed;
            OffOnInput.SetActive(false);
        } else if (Input.GetKey(UpKey)) {
        }
        rigidbody.velocity = velocity;
    }
}
