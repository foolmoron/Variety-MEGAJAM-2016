using UnityEngine;
using System.Collections;

public class Question : MonoBehaviour {

    [Range(0, 80)]
    public int WholePart;
    [Range(0, 9)]
    public int DecimalTensPart;
    [Range(1, 5)]
    public int Multiplier;

    [Range(0, 360)]
    public float MinRotation;
    [Range(0, 360)]
    public float MaxRotation;
    [Range(0, 10)]
    public float MinFallingSpeed = 0.2f;
    [Range(0, 10)]
    public float MaxFallingSpeed = 3;

    TextMesh text;

    Rotating rotating;
    Moving moving;

    void Start() {
        text = GetComponentInChildren<TextMesh>();

        rotating = GetComponent<Rotating>();
        moving = GetComponent<Moving>();

        Randomize();
    }

    public void Randomize() {
        // generate random parts
        {
            WholePart = Random.Range(0, 80);
            DecimalTensPart = Random.Range(0, 9);
            Multiplier = Random.Range(1, 5);
        }
        // random color
        {
            text.color = new HSBColor(Random.value, 1, 0.36f).ToColor();
        }
        // falling speed based on part complexity (i.e. "difficulty")
        {
            var complexity = WholePart * DecimalTensPart * Multiplier;
            var difficulty = complexity / 2500;
            moving.MovementPerSecond.y = -Mathf.Lerp(MaxFallingSpeed, MinFallingSpeed, difficulty);
        }
        // random rotation
        {
            rotating.DegreesPerSecond.z = Random.Range(MinRotation, MaxRotation) * (Random.value > 0.5f ? -1 : 1);
            rotating.DegreesPerSecond.z = Random.Range(MinRotation, MaxRotation) * (Random.value > 0.5f ? -1 : 1);
        }
    }

    public float GetSolution() {
        return (WholePart + ((float)DecimalTensPart / 10)) * Multiplier;
    }

    void Update() {
        // set text values based on parts
        {
            text.text = "$" + WholePart + "." + (DecimalTensPart) + "0" + "x" + Multiplier;
        }
    }
}
