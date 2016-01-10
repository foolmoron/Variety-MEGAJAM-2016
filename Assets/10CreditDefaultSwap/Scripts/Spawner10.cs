using UnityEngine;
using System.Collections;

public class Spawner10 : MonoBehaviour {

    public GameObject SpawnPrefab;

    [Range(0, 1)]
    public float Difficulty = 0;
    [Range(0, 10)]
    public float MinSecondsPerSpawn = 0.1f;
    [Range(0, 10)]
    public float MaxSecondsPerSpawn = 1f;
    [Range(0, 10)]
    public float MinSpawnSpeed = 0.5f;
    [Range(0, 10)]
    public float MaxSpawnSpeed = 5f;
    [Range(0, 1)]
    public float SpawnSpeedVariance = 0.25f;

    Rect randomDroppingArea;
    float timeSinceLastSpawn;

    void Start() {
        // get random dropping area from the box collider's bounds
        {
            var boxCollider = GetComponent<BoxCollider2D>();
            randomDroppingArea = new Rect(boxCollider.offset - boxCollider.size / 2, boxCollider.size);
        }
    }
    
    void Update() {
        var secondsPerSpawn = Mathf.Lerp(MaxSecondsPerSpawn, MinSecondsPerSpawn, Difficulty);
        // drop new question
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= secondsPerSpawn) { 
                var randomOffset = new Vector2(Random.Range(randomDroppingArea.xMin, randomDroppingArea.xMax), Random.Range(randomDroppingArea.yMin, randomDroppingArea.yMax));
                var spawn = (GameObject) Instantiate(SpawnPrefab, transform.position + randomOffset.to3(), Quaternion.identity);

                var speed = Mathf.Lerp(MinSpawnSpeed, MaxSpawnSpeed, Difficulty) * (1 + (Random.value * SpawnSpeedVariance - SpawnSpeedVariance / 2));
                spawn.GetComponent<Moving>().MovementPerSecond = spawn.GetComponent<Moving>().MovementPerSecond * speed;

                spawn.GetComponent<GetColored10>().Inverted = Random.value <= 0.5f;
                
                timeSinceLastSpawn -= secondsPerSpawn;
            }
        }
    }
}
