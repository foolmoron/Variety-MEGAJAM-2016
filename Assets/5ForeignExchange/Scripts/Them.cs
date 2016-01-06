using UnityEngine;
using System.Collections;

public class Them : MonoBehaviour {

    public string Name;
    public GameObject MessagePrefab;
    [Range(0, 100)]
    public int MaxMessageLength = 48;

    [Range(0, 30)]
    public float MinWaitTime;
    [Range(0, 30)]
    public float MaxWaitTime;
    float waitingTime;

    MarkovChatbot markov;
    MessageBox messageBox;

    public AudioClip SubmitSound;

    void Start() {
        markov = FindObjectOfType<MarkovChatbot>();
        messageBox = FindObjectOfType<MessageBox>();
    }

    public void ActuallyRespond() {
        var message = Instantiate(MessagePrefab);
        message.GetComponent<Message>().Name = Name;
        message.GetComponent<Message>().Text = markov.GetMessageOfLength(Mathf.CeilToInt(Mathf.Lerp(MaxMessageLength * 0.33f, MaxMessageLength, Random.value)));
        messageBox.AddMessage(message.transform);
        AudioSource.PlayClipAtPoint(SubmitSound, Vector3.zero);
    }

    public void AskForResponse() {
        if (waitingTime <= 0) {
            waitingTime = Mathf.Lerp(MinWaitTime, MaxWaitTime, Random.value);
        } else {
            waitingTime /= 2;
        }
    }

    void Update() {
        if (waitingTime > 0) {
            waitingTime -= Time.deltaTime;
            if (waitingTime <= 0) {
                ActuallyRespond();
            }
        }
    }
}