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
    readonly Dictionary<string, List<string>> secondOrderMarkov = new Dictionary<string, List<string>>();
    readonly Dictionary<string, List<string>> thirdOrderMarkov = new Dictionary<string, List<string>>();

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
                    string previousPreviousWord = null;
                    string previousWord = null;
                    for (int w = 0; w < words.Length - 1; w++) {
                        var word = words[w];
                        firstOrderMarkov[word] = firstOrderMarkov.ContainsKey(word) ? firstOrderMarkov[word] : new List<string>();
                        firstOrderMarkov[word].Add(words[w + 1]);
                        if (previousWord != null) {
                            var combinedWord = previousWord + word;
                            secondOrderMarkov[combinedWord] = secondOrderMarkov.ContainsKey(combinedWord) ? secondOrderMarkov[combinedWord] : new List<string>();
                            secondOrderMarkov[combinedWord].Add(words[w + 1]);
                        }
                        if (previousPreviousWord != null) {
                            var combinedWord = previousPreviousWord + previousWord + word;
                            thirdOrderMarkov[combinedWord] = thirdOrderMarkov.ContainsKey(combinedWord) ? thirdOrderMarkov[combinedWord] : new List<string>();
                            thirdOrderMarkov[combinedWord].Add(words[w + 1]);
                        }
                        previousWord = words[w];
                        if (w > 1) {
                            previousPreviousWord = words[w - 1];
                        }
                    }
                }
            }
        }
    }

    public string GetMessageOfLength(int length) {
        var sb = new StringBuilder();
        var previousPreviousWord = "";
        var previousWord = "";
        var currentWord = "";
        var attempts = 0;
        while (sb.Length < length || attempts > 1000) {
            //if (thirdOrderMarkov.ContainsKey(previousPreviousWord + previousWord + currentWord)) {
            //    var list = thirdOrderMarkov[previousPreviousWord + previousWord + currentWord];
            //    var randomWord = list[Mathf.FloorToInt(Random.value * list.Count)];
            //    previousPreviousWord = previousWord;
            //    previousWord = currentWord;
            //    currentWord = randomWord;
            //    sb.Append(randomWord);
            //    if (randomWord.Length > 0) {
            //        sb.Append(" ");
            //    }
            //} else if (secondOrderMarkov.ContainsKey(previousWord + currentWord)) {
            //    var list = secondOrderMarkov[previousWord + currentWord];
            //    var randomWord = list[Mathf.FloorToInt(Random.value * list.Count)];
            //    previousPreviousWord = previousWord;
            //    previousWord = currentWord;
            //    currentWord = randomWord;
            //    sb.Append(randomWord);
            //    if (randomWord.Length > 0) {
            //        sb.Append(" ");
            //    }
            //} else 
            if (firstOrderMarkov.ContainsKey(currentWord)) {
                var list = firstOrderMarkov[currentWord];
                var randomWord = list[Mathf.FloorToInt(Random.value * list.Count)];
                previousPreviousWord = previousWord;
                previousWord = currentWord;
                currentWord = randomWord;
                sb.Append(randomWord);
                if (randomWord.Length > 0) {
                    sb.Append(" ");
                }
            } else {
                previousPreviousWord = "";
                previousWord = "";
                currentWord = "";
            }
            attempts++;
        }
        return sb.ToString().Trim();
    }
}