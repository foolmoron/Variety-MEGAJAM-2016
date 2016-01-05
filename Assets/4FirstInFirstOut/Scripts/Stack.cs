using UnityEngine;
using System.Collections;

public class Stack : MonoBehaviour {

    public FollowHeight FollowHeight;
    public bool IsAtTop;
    public float HeightLanded = float.NegativeInfinity;

    void Start() {
    }

    void Update() {
        // check if the stack tipped
        {
            if (transform.position.y < (HeightLanded - 0.1f)) {
                Debug.Log("TIPPED");
            }
        }
    }
    
    public void OnCollisionEnter2D(Collision2D collision) {
        var stack = collision.gameObject.GetComponent<Stack>();
        if (stack && stack.IsAtTop) {
            stack.IsAtTop = false;
            IsAtTop = true;
            HeightLanded = transform.position.y;
            FollowHeight.Height = transform.position.y;
        }
    }
}
