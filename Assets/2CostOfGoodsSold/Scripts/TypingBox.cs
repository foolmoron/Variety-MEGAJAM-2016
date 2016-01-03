using UnityEngine;
using System.Collections;

public class TypingBox : MonoBehaviour {

    public static readonly KeyCode[] ALLOWED_INPUTS = {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Period, KeyCode.Return,
        KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.KeypadPeriod, KeyCode.KeypadEnter,
        KeyCode.Backspace, KeyCode.Delete,
    };

    [Range(1, 10)]
    public int MaxInputLength = 6;
    TextMesh moneyInput;

    public float UnderscoreStepX;
    float underscoreInitialX;
    TextMesh underscore;

    public GameObject PopupTextPrefab;
    public Vector3 PopupTextOffset;

    QuestionDropper questionDropper;
    TextMesh[] allTexts;

    public float FreezeInputTime;

    public bool IsLoan;
    public Color MainColor;
    public Color LoanColor;

    public AudioClip TypeSound;
    public AudioClip CorrectSound;

    void Start() {
        allTexts = GetComponentsInChildren<TextMesh>();

        questionDropper = FindObjectOfType<QuestionDropper>();
        questionDropper.OnGameOver += () => {
            IsLoan = true;
            FreezeInputTime = 3;
            moneyInput.text = "";
        };

        moneyInput = transform.FindChild("Input").GetComponent<TextMesh>();
        moneyInput.text = "";

        underscore = transform.FindChild("_").GetComponent<TextMesh>();
        underscoreInitialX = underscore.transform.position.x;
    }

    public float GetNumber(string numberText) {
        var firstNumber = string.IsNullOrEmpty(numberText) ? 0 : float.Parse(numberText);
        var currencyFormatted = firstNumber.ToString("C");
        return float.Parse(currencyFormatted.Substring(1));
    }

    public void EnterNumber(string numberText) {
        var finalNumber = GetNumber(numberText);
        // popup text
        {
            var popupText = (GameObject)Instantiate(PopupTextPrefab, transform.position + PopupTextOffset, Quaternion.identity);
            popupText.GetComponent<TextMesh>().text = finalNumber.ToString("C");
        }
        // kill numbers that match
        {
            for (int i = 0; i < questionDropper.CurrentQuestions.Count; i++) {
                var question = questionDropper.CurrentQuestions[i];
                if (finalNumber == question.GetSolution()) {
                    Destroy(question.gameObject);
                    questionDropper.CurrentQuestions.RemoveAt(i);
                    i--;
                    questionDropper.Money += finalNumber;
                    questionDropper.QuestionsRight++;
                    AudioSource.PlayClipAtPoint(CorrectSound, Vector3.zero);
                }
            }
        }
    }

    void Update() {
        // freeze input
        {
            if (FreezeInputTime > 0) {
                FreezeInputTime -= Time.deltaTime;
            }
        }
        // process inputs
        {
            if (FreezeInputTime <= 0) {
                for (int i = 0; i < ALLOWED_INPUTS.Length; i++) {
                    var key = ALLOWED_INPUTS[i];
                    if (Input.GetKeyDown(key)) {
                        var text = moneyInput.text;
                        switch (key) {
                            case KeyCode.Backspace:
                            case KeyCode.Delete:
                                if (text.Length > 0) {
                                    text = text.Substring(0, text.Length - 1);
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                }
                                break;
                            case KeyCode.Return:
                            case KeyCode.KeypadEnter:
                                if (IsLoan) {
                                    if (GetNumber(text) > 0) {
                                        questionDropper.Reset(GetNumber(text));
                                        text = "";
                                        IsLoan = false;
                                        AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    }
                                } else {
                                    EnterNumber(text);
                                    text = "";
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                }
                                break;
                        }
                        if (text.Length < MaxInputLength) {
                            switch (key) {

                                case KeyCode.Alpha0:
                                case KeyCode.Keypad0:
                                    text += 0;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha1:
                                case KeyCode.Keypad1:
                                    text += 1;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha2:
                                case KeyCode.Keypad2:
                                    text += 2;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha3:
                                case KeyCode.Keypad3:
                                    text += 3;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha4:
                                case KeyCode.Keypad4:
                                    text += 4;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha5:
                                case KeyCode.Keypad5:
                                    text += 5;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha6:
                                case KeyCode.Keypad6:
                                    text += 6;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha7:
                                case KeyCode.Keypad7:
                                    text += 7;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha8:
                                case KeyCode.Keypad8:
                                    text += 8;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Alpha9:
                                case KeyCode.Keypad9:
                                    text += 9;
                                    AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    break;
                                case KeyCode.Period:
                                case KeyCode.KeypadPeriod:
                                    if (!text.Contains(".")) {
                                        text += '.';
                                        AudioSource.PlayClipAtPoint(TypeSound, Vector3.zero);
                                    }
                                    break;
                            }
                        }
                        moneyInput.text = text;
                    }
                }
            }
        }
        // position underscore based on text length
        {
            underscore.transform.position = underscore.transform.position.withX(underscoreInitialX + UnderscoreStepX * moneyInput.text.Length);
        }
        // show underscore based on text length & freeze status
        {
            underscore.gameObject.SetActive(moneyInput.text.Length < MaxInputLength && FreezeInputTime <= 0);
        }
        // text color based on loan status
        {
            for (int i = 0; i < allTexts.Length; i++) {
                allTexts[i].color = IsLoan ? LoanColor : MainColor;
            }
        }
    }
}