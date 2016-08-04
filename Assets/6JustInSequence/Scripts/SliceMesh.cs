using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SliceMesh : MonoBehaviour {

    public Color Color;
    Mesh mesh;
    Color[] colors = new Color[3];

    void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = new[] {
            new Vector3(0, 0),
            new Vector3(Mathf.Cos(9*Mathf.Deg2Rad), Mathf.Sin(9*Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(-9*Mathf.Deg2Rad), Mathf.Sin(-9*Mathf.Deg2Rad)),
        };
        mesh.uv = new[] {
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0),
        };
        mesh.triangles = new [] { 0, 1, 2 };
    }

    void Update() {
        for (int i = 0; i < colors.Length; i++) {
            colors[i] = Color;
        }
        mesh.colors = colors;
    }
}