using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour {
    
    public int MessageCount;
    [Range(0, 2)]
    public float MessageGap = 0.5f;

    Vector3 originalPos;

    void Start() {
        originalPos = transform.position;
    }

    public void AddMessage(Transform messageRoot) {
        messageRoot.parent = transform;
        messageRoot.localPosition = new Vector3(0, MessageCount * -MessageGap, 0);
        MessageCount++;
        transform.position = originalPos.plusY(MessageCount * MessageGap);
    }

    void Update() {
    }
}