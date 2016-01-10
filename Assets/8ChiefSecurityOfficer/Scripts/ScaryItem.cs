using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScaryItem : MonoBehaviour {

    public AnimationCurve DistanceToVolume;
    public AnimationCurve DistanceToPitch;

    Player8 player;
    new AudioSource audio;

    void Start() {
        player = FindObjectOfType<Player8>();
        audio = GetComponent<AudioSource>();
    }

    void Update() {
        var dist = (player.transform.position - transform.position).magnitude;
        audio.volume = DistanceToVolume.Evaluate(dist);
        audio.pitch = DistanceToPitch.Evaluate(dist);
    }
}
