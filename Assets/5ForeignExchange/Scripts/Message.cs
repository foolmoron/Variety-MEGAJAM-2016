using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

    public string Text;
    TextMesh message;

    void Start() {
        message = transform.FindChild("Message").GetComponent<TextMesh>();
    }

    void Update() {
        // set message
        {
            message.text = Text;
        }
    }
}