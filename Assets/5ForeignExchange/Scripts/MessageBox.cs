using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour {
    
    public int MessageCount;
    [Range(0, 2)]
    public float MessageGap = 0.5f;

    public Vector3 TargetPosition;
    [Range(0, 0.25f)]
    public float Speed = 0.2f;
    Vector3 originalPos;

    void Awake() {
        originalPos = transform.position;
    }

    public void AddMessage(Transform messageRoot) {
        messageRoot.parent = transform;
        messageRoot.localPosition = new Vector3(0, MessageCount * -MessageGap, 0);
        MessageCount++;
        TargetPosition = originalPos.plusY(MessageCount * MessageGap);
    }

    void Update() {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Speed);
    }
}