using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class MarkovChatbot : MonoBehaviour {

    public TextAsset TrainingText;

    readonly Dictionary<string, List<string>> firstOrderMarkov = new Dictionary<string, List<string>> {
        { "", new List<string> { "i", "uh" } }
    };

    void Start() {
        Train(TrainingText.text.Split('\n'));
    }

    public void Train(string[] input) {
        // strip punctuation and make lower
        {
            for (int i = 0; i < input.Length; i++) {
                input[i] = Regex.Replace(input[i], "[^\\d\\w ]", "").ToLower();
            }
        }
        // parse words into markov
        {
            for (int i = 0; i < input.Length; i++) {
                var words = input[i].Split(' ');
                if (words.Length > 0) {
                    firstOrderMarkov[""].Add(words[0]);
                    for (int w = 0; w < words.Length - 1; w++) {
                        var word = words[w];
                        firstOrderMarkov[word] = firstOrderMarkov.ContainsKey(word) ? firstOrderMarkov[word] : new List<string>();
                        firstOrderMarkov[word].Add(words[w + 1]);
                    }
                }
            }
        }
    }

    public string GetMessageOfLength(int length) {
        var sb = new StringBuilder();
        var currentWord = "";
        var attempts = 0;
        while (sb.Length < length || attempts > 1000) {
            if (firstOrderMarkov.ContainsKey(currentWord)) {
                var list = firstOrderMarkov[currentWord];
                var randomWord = list[Mathf.FloorToInt(Random.value * list.Count)];
                sb.Append(randomWord);
                if (randomWord.Length > 0) {
                    sb.Append(" ");
                }
            } else {
                currentWord = "";
            }
            attempts++;
        }
        return sb.ToString().Trim();
    }
}