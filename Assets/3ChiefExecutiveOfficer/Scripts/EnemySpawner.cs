using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    [Range(0, 10)]
    public float SpawnInterval = 4f;
    [Range(-1, 1)]
    public float SpawnIntervalVelocity = -0.05f;
    float spawnTime = float.PositiveInfinity;

    public Transform Target;
    public GameObject SpawnPrefab;
    [Range(0, 5)]
    public float SpawnMinDistance = 1f;
    [Range(0, 10)]
    public float SpawnMaxDistance = 5f;
    [Range(0, 100)]
    public int InitialSpawn = 10;

    void Start() {
        for (int i = 0; i < InitialSpawn; i++) {
            Spawn();
        }
    }

    public void Spawn() {
        var enemy = Instantiate(SpawnPrefab).GetComponent<Enemy>();
        enemy.Target = Target;

        var randomAngle = Random.value * Mathf.PI * 2;
        var direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        var position = transform.position + direction.to3() * Mathf.Lerp(SpawnMinDistance, SpawnMaxDistance, Random.value);
        enemy.transform.position = position;
    }

    void Update() {
        // spawn interval velocity
        {
            SpawnInterval += SpawnIntervalVelocity * Time.deltaTime;
        }
        // spawn
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= SpawnInterval) {
                Spawn();
                spawnTime = 0;
            }
        }
    }
}
