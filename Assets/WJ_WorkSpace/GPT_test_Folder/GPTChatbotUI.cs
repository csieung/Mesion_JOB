using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GPTChatbotUI : MonoBehaviour
{
    public GPTChatbot gptChatbot;
    public TMP_InputField inputField;
    public TMP_Text responseText;
    public Button[] choiceButtons; // 선택지 버튼

    private void Start()
    {
        foreach (Button button in choiceButtons)
        {
            if (button != null)
            {
                Button localButton = button; // 지역 변수를 사용하여 클로저 문제를 피합니다.
                localButton.onClick.AddListener(() => OnChoiceButtonClicked(localButton));
            }
        }
    }

    public void OnSendButtonClicked()
    {
        string userInput = inputField.text;
        Debug.Log("Send Button Clicked. User Input: " + userInput);
        gptChatbot.SendInitialRequest(userInput);
    }

    public void OnChoiceButtonClicked(Button clickedButton)
    {
        // 버튼의 텍스트를 UserInputField로 전송하지 않고, 바로 GPT에 요청합니다.
        string choiceText = clickedButton.GetComponentInChildren<TMP_Text>().text;
        Debug.Log("Button clicked with text: " + choiceText);
        gptChatbot.SendFollowUpRequest(choiceText);
    }

    public void DisplayGPTResponse(string gptResponse)
    {
        responseText.text = gptResponse;
    }

    public void DisplayChoices(string[] choices)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = choices[i];
                choiceButtons[i].gameObject.SetActive(true);
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }
}