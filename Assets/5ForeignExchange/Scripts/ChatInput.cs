using UnityEngine;
using System.Collections;

public class ChatInput : MonoBehaviour {
    
    [Range(1, 200)]
    public int MaxInputLength = 6;

    TextMesh input;

    public AudioClip TypeSound;
    public AudioClip SubmitSound;

    public GameObject MessagePrefab;
    MessageBox messageBox;
    Them them;

    void Start() {
        messageBox = FindObjectOfType<MessageBox>();
        them = FindObjectOfType<Them>();
        input = transform.FindChild("Input").GetComponent<TextMesh>();
        input.text = "";
    }

    void Update() {
        // process inputs
        {
            var text = input.text;
            var str = Input.inputString;
            var ch = str.Length > 0 ? str[0] : 0;
            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)) {
                if (text.Length > 0) {
                    text = text.Substring(0, text.Length - 1);
                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                }
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                var message = Instantiate(MessagePrefab);
                message.GetComponent<Message>().Text = text;
                messageBox.AddMessage(message.transform);
                text = "";
                AudioSource.PlayClipAtPoint(SubmitSound, Vector3.zero);
                them.AskForResponse(); // this line feels really creepy for some reason
            } else if (32 <= ch && ch <= 126 && text.Length < MaxInputLength) {
                text += str;
                AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
            }
            input.text = text;
        }
    }
}