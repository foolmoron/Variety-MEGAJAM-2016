using UnityEngine;
using System.Collections;

public class Them : MonoBehaviour {

    public string Name;
    public string Country;
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

    public TextAsset NamePrefixes;
    public TextAsset NameSuffixes;
    public TextAsset CountryPrefixes;
    public TextAsset CountrySuffixes;

    public AudioClip SubmitSound;

    void Start() {
        markov = FindObjectOfType<MarkovChatbot>();
        messageBox = FindObjectOfType<MessageBox>();

        // generate name
        {
            var prefixes = NamePrefixes.text.Split('\n');
            var suffixes = NameSuffixes.text.Split('\n');
            Name = prefixes.Random() + suffixes.Random();
        }
        // generate country
        {
            var prefixes = CountryPrefixes.text.Split('\n');
            var suffixes = CountrySuffixes.text.Split('\n');
            Country = prefixes.Random() + suffixes.Random();
        }


        var message = Instantiate(MessagePrefab);
        message.GetComponent<Message>().Name = Name;
        message.GetComponent<Message>().Text = "hello i am " + Name + " from " + Country + " whats your name";
        messageBox.AddMessage(message.transform);
        AudioSource.PlayClipAtPoint(SubmitSound, Vector3.zero);
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