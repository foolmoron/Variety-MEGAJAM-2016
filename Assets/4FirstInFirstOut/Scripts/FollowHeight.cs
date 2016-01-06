using UnityEngine;
using System.Collections;

public class FollowHeight : MonoBehaviour {

    public float Height;
    public float Offset = -1;
    public float MinHeight = 0;
    [Range(0, 0.25f)]
    public float Speed = 0.2f;

    new Camera camera;

    void Start() {
        camera = GetComponent<Camera>();
    }

    void Update() {
        var height = Mathf.Max(Height + Offset + camera.orthographicSize, MinHeight);
        transform.position = transform.position.withY(Mathf.Lerp(transform.position.y, height, Speed));
    }
}
