using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

    public string Name;
    TextMesh nameText;

    public string Text;
    TextMesh messageText;

    void Start() {
        nameText = transform.Find("Name").GetComponent<TextMesh>();
        messageText = transform.Find("Message").GetComponent<TextMesh>();
        nameText.text = Name;
        messageText.text = Text;
    }

    void Update() {
        // reset texts
        {
            nameText.text = Name;
            messageText.text = Text;
        }
    }
}