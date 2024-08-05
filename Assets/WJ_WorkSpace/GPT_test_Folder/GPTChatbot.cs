using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;

public class GPTChatbot : MonoBehaviour
{
    private string apiKey = "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";
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    [SerializeField]
    private GPTChatbotUI uiHandler;

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class GPTRequest
    {
        public string model;
        public List<Message> messages;
        public int max_tokens;
        public int n; // 응답 개수 설정
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
    }

    [System.Serializable]
    public class GPTResponse
    {
        public List<Choice> choices;
    }

    private List<Message> conversationHistory = new List<Message>();
    private string conversationFilePath; // 파일 경로

    private const int maxConversationLength = 10; // 유지할 최대 메시지 개수
    private void Awake()
    {
        conversationFilePath = Path.Combine(Application.persistentDataPath, "conversationHistory.json");
        Debug.Log("Conversation file path: " + conversationFilePath);
    }
    private void Start()
    {
        LoadConversationHistory();
        Debug.Log(conversationFilePath);
    }

    // 대화 기록을 파일에 저장하는 메서드
    private void SaveConversationHistory()
    {
        string json = JsonConvert.SerializeObject(conversationHistory);
        File.WriteAllText(conversationFilePath, json);
    }

    // 저장된 대화 기록을 불러오는 메서드
    private void LoadConversationHistory()
    {
        if (File.Exists(conversationFilePath))
        {
            string json = File.ReadAllText(conversationFilePath);
            conversationHistory = JsonConvert.DeserializeObject<List<Message>>(json);
        }
    }

    // 대화 기록을 관리하는 메서드
    private void ManageConversationHistory()
    {
        if (conversationHistory.Count > maxConversationLength)
        {
            conversationHistory.RemoveAt(0); // 가장 오래된 메시지 삭제
        }
    }

    public void SendInitialRequest(string prompt)
    {
        if (uiHandler == null)
        {
            Debug.LogError("UI Handler is not assigned in the Inspector.");
            return;
        }

        conversationHistory.Clear();
        conversationHistory.Add(new Message { role = "user", content = prompt });

        GPTRequest request = new GPTRequest
        {
            model = "gpt-3.5-turbo",
            messages = new List<Message>(conversationHistory),
            max_tokens = 300,
            n = 1 // 한 개의 응답을 요청
        };

        string jsonRequest = JsonConvert.SerializeObject(request);
        StartCoroutine(PostRequest(apiUrl, jsonRequest, true));
    }

    public void SendFollowUpRequest(string userInput)
    {
        conversationHistory.Add(new Message { role = "user", content = userInput });
        ManageConversationHistory(); // 대화 기록 관리

        GPTRequest request = new GPTRequest
        {
            model = "gpt-3.5-turbo",
            messages = new List<Message>(conversationHistory),
            max_tokens = 200,
            n = 1 // 한 개의 응답을 요청
        };

        string jsonRequest = JsonConvert.SerializeObject(request);
        StartCoroutine(PostRequest(apiUrl, jsonRequest, false));
    }

    IEnumerator PostRequest(string url, string json, bool isInitialRequest)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            if (string.IsNullOrEmpty(jsonResponse))
            {
                Debug.LogError("Response is empty");
                yield break;
            }

            GPTResponse response = JsonConvert.DeserializeObject<GPTResponse>(jsonResponse);

            if (response == null || response.choices == null || response.choices.Count == 0)
            {
                Debug.LogError("Invalid response");
                yield break;
            }

            string gptResponseText = response.choices[0]?.message?.content;

            if (gptResponseText == null)
            {
                Debug.LogError("Response text is null");
            }
            else
            {
                conversationHistory.Add(new Message { role = "assistant", content = gptResponseText });
                ManageConversationHistory(); // 대화 기록 관리
                SaveConversationHistory(); // 대화 기록 저장
                uiHandler.DisplayGPTResponse(gptResponseText);

                SendFollowUpRequestForChoices(gptResponseText);
            }
        }
    }

    public void SendFollowUpRequestForChoices(string gptResponse)
    {
        List<Message> messages = new List<Message>
        {
            new Message { role = "system", content = "You are a helpful assistant. Provide four possible user responses to the following assistant response." },
            new Message { role = "assistant", content = gptResponse }
        };

        GPTRequest request = new GPTRequest
        {
            model = "gpt-3.5-turbo",
            messages = messages,
            max_tokens = 300,
            n = 1 // 한 개의 응답을 요청
        };

        string jsonRequest = JsonConvert.SerializeObject(request);
        StartCoroutine(PostRequestForChoices(apiUrl, jsonRequest));
    }

    IEnumerator PostRequestForChoices(string url, string json)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            if (string.IsNullOrEmpty(jsonResponse))
            {
                Debug.LogError("Response is empty");
                yield break;
            }

            GPTResponse response = JsonConvert.DeserializeObject<GPTResponse>(jsonResponse);

            if (response == null || response.choices == null || response.choices.Count == 0)
            {
                Debug.LogError("Invalid response");
                yield break;
            }

            string gptResponseText = response.choices[0]?.message?.content;

            if (gptResponseText == null)
            {
                Debug.LogError("Response text is null");
            }
            else
            {
                string[] choices = ParseChoices(gptResponseText);
                uiHandler.DisplayChoices(choices);
            }
        }
    }

    private string[] ParseChoices(string rawResponse)
    {
        // 응답을 분리하여 각 선택지로 나눕니다.
        string[] separators = new string[] { "1.", "2.", "3.", "4." };
        List<string> choices = new List<string>();

        for (int i = 0; i < separators.Length; i++)
        {
            int startIndex = rawResponse.IndexOf(separators[i]);
            int endIndex = (i + 1 < separators.Length) ? rawResponse.IndexOf(separators[i + 1]) : rawResponse.Length;

            if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
            {
                string choice = rawResponse.Substring(startIndex, endIndex - startIndex).Trim();
                if (!string.IsNullOrEmpty(choice))
                {
                    choices.Add(choice);
                }
            }
        }

        return choices.ToArray();
    }
}