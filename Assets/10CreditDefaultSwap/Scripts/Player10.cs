using UnityEngine;
using System.Collections;

public class Player10 : MonoBehaviour {

    [Range(0, 10)]
    public float Speed = 2;
    [Range(0, 0.25f)]
    public float RotationSpeed = 0.1f;

    Vector2 lastDirection;
    
    public float TimeAlive;
    public AnimationCurve TimeToDifficulty;
    public GameObject GameOverObject;
    public TextMesh ScoreText;

    new Rigidbody2D rigidbody;

    Colorizer10 colorizer;
    Spawner10[] spawners;

    public AudioClip LoseSound;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        colorizer = Colorizer10.instance;
        spawners = FindObjectsOfType<Spawner10>();
    }

    void Update() {
        var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (direction != Vector2.zero) {
            lastDirection = direction;
        }
        // move in direction
        {
            rigidbody.AddForce(direction * Speed);
        }
        // rotate to point to last direction
        {
            var desiredAngle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg - 90;
            transform.localRotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.localRotation.eulerAngles.z, desiredAngle, RotationSpeed));
        }
        // time alive
        {
            TimeAlive += Time.deltaTime;
        }
        // spawner difficulty
        {
            var difficulty = TimeToDifficulty.Evaluate(TimeAlive);
            for (int i = 0; i < spawners.Length; i++) {
                spawners[i].Difficulty = difficulty;
            }
        }
    }

    public void GameOver() {
        var movers = FindObjectsOfType<Moving>();
        for (int i = 0; i < movers.Length; i++) {
            movers[i].enabled = false;
        }
        for (int i = 0; i < spawners.Length; i++) {
            Destroy(spawners[i]);
        }
        GameOverObject.SetActive(true);
        ScoreText.text = "You survived\n" + TimeAlive.ToString("0.00") + " seconds";
        rigidbody.velocity = Vector2.zero;
        enabled = false; // no more control for player
        AudioSource.PlayClipAtPoint(LoseSound, Vector3.zero);
        StartCoroutine(Grayness());
    }

    IEnumerator Grayness() {
        while (true) {
            colorizer.Color.s = Mathf.Lerp(colorizer.Color.s, 0, 0.03f);
            colorizer.Color.b = Mathf.Lerp(colorizer.Color.b, 0.75f, 0.03f);
            yield return new WaitForEndOfFrame();
        }
    }
}
