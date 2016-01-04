using UnityEngine;
using System.Collections;

public class Stack : MonoBehaviour {

    public FollowHeight FollowHeight;
    public bool IsAtTop;

    void Start() {
    }
    
    public void OnCollisionEnter2D(Collision2D collision) {
        var stack = collision.gameObject.GetComponent<Stack>();
        if (stack && stack.IsAtTop) {
            stack.IsAtTop = false;
            IsAtTop = true;
            FollowHeight.Height = transform.position.y;
        }
    }
}
