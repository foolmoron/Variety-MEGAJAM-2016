using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

    public string Name;
    TextMesh name;

    public string Text;
    TextMesh message;

    void Start() {
        name = transform.FindChild("Name").GetComponent<TextMesh>();
        message = transform.FindChild("Message").GetComponent<TextMesh>();
        name.text = Name;
        message.text = Text;
    }

    void Update() {
        // reset texts
        {
            name.text = Name;
            message.text = Text;
        }
    }
}