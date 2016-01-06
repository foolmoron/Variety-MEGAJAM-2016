using UnityEngine;
using System.Collections;

public class Them : MonoBehaviour {

    public string Name;
    public GameObject MessagePrefab;
    [Range(0, 100)]
    public int MaxMessageLength = 48;

    MarkovChatbot markov;
    MessageBox messageBox;

    void Start() {
        markov = FindObjectOfType<MarkovChatbot>();
        messageBox = FindObjectOfType<MessageBox>();
    }

    public void Respond() {
        var message = Instantiate(MessagePrefab);
        message.GetComponent<Message>().Name = Name;
        message.GetComponent<Message>().Text = markov.GetMessageOfLength(Mathf.CeilToInt(Mathf.Lerp(MaxMessageLength * 0.33f, MaxMessageLength, Random.value)));
        messageBox.AddMessage(message.transform);
    }
}