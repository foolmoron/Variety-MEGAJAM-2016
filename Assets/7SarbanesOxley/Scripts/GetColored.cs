using UnityEngine;
using System.Collections;

public class GetColored : MonoBehaviour {

    public SpriteRenderer TargetSprite;

    public void OnTriggerEnter2D(Collider2D collision) {
        var colorer = collision.GetComponent<Colorer>();
        if (colorer) {
            TargetSprite.color = colorer.Color;
        }
    }
}
