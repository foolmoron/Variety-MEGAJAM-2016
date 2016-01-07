using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using System.Collections;

public class GhostPulser : MonoBehaviour {

    [Range(0.001f, 3f)]
    public float Duration = 0.5f;
    public Vector2 MoveAmount = new Vector2(0, 0);
    public Vector2 ScaleAmount = new Vector2(0, 0);

    SpriteRenderer spriteRenderer;

    // parallel lists
    List<GameObject> ghostObjects = new List<GameObject>(10);
    List<float> ghostDurations = new List<float>(10);

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        // destroy ghosts after they run out of duration
        {
            for (int i = 0; i < ghostDurations.Count; i++) {
                ghostDurations[i] -= Time.deltaTime;
                if (ghostDurations[i] <= 0) {
                    Destroy(ghostObjects[i]);
                    ghostObjects.RemoveAt(i);
                    ghostDurations.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    
    public void Pulse() {
        Pulse(Duration, MoveAmount, ScaleAmount);
    }

    public void Pulse(float duration, Vector2 moveAmount, Vector2 scaleAmount) {
        if (spriteRenderer.enabled == false || spriteRenderer.sprite == null)
            return;

        // create new ghost
        GameObject newObj = null;
        {
            newObj = new GameObject(gameObject.name);
            newObj.transform.position = transform.position.plusZ(-0.00001f);
            newObj.transform.rotation = transform.rotation;
            var newSprite = newObj.AddComponent<SpriteRenderer>();
            newSprite.sprite = spriteRenderer.sprite;
            newSprite.color = spriteRenderer.color;

            newObj.transform.parent = transform.parent;
            newObj.transform.localScale = transform.localScale;
        }
        // start tweening ghost
        {
            Tween.MoveBy(newObj, new Vector3(moveAmount.x, moveAmount.y, 0), duration, Interpolate.EaseType.EaseOutCirc);
            Tween.ScaleBy(newObj, new Vector3(scaleAmount.x, scaleAmount.y, 0), duration, Interpolate.EaseType.EaseOutCirc);
            Tween.ColorTo(newObj, spriteRenderer.color.withAlpha(0), duration, Interpolate.EaseType.Linear);
        }
        // register new ghost to keep track of when to destroy it
        {
            ghostObjects.Add(newObj);
            ghostDurations.Add(duration);
        }
    }

    void OnDisable() {
        // destroy all ghosts
        for (int i = 0; i < ghostObjects.Count; i++) {
            Destroy(ghostObjects[i]);
        }
        ghostObjects.Clear();
        ghostDurations.Clear();
    }
}
