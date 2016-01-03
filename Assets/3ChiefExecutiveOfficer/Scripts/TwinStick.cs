using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwinStick : MonoBehaviour {

    [Range(0, 10)]
    public float Speed = 2;

    [Range(0, 3)]
    public float ShootInterval = 0.5f;
    float shootTime;
    public GameObject BulletPrefab;

    AudioSource shootSound;
    
    void Start() {
        shootSound = GetComponent<AudioSource>();
    }
    
    void Update() {
        // move based on WASD
        {
            var direction = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) direction.y = 1;
            if (Input.GetKey(KeyCode.S)) direction.y = -1;
            if (Input.GetKey(KeyCode.D)) direction.x = 1;
            if (Input.GetKey(KeyCode.A)) direction.x = -1;
            transform.position += direction.normalized.to3() * Speed * Time.deltaTime;
        }
        // shoot based on IJKL
        {
            var direction = Vector2.zero;
            if (Input.GetKey(KeyCode.I)) direction.y = 1;
            if (Input.GetKey(KeyCode.K)) direction.y = -1;
            if (Input.GetKey(KeyCode.L)) direction.x = 1;
            if (Input.GetKey(KeyCode.J)) direction.x = -1;

            shootTime += Time.deltaTime;
            if (direction != Vector2.zero && shootTime >= ShootInterval) {
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var bullet = (GameObject)Instantiate(BulletPrefab, transform.position.plusZ(-0.1f), Quaternion.Euler(0, 0, angle));
                shootTime = 0;
                shootSound.pitch = Random.Range(0.9f, 1.1f);
                shootSound.Play();
            }
        }
    }
}
