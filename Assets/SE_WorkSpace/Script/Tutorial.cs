using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialPanel; // 튜토리얼 패널
    public GameObject DroneUIPanel; // 튜토리얼 패널
    public TextMeshProUGUI tutorialText; // 튜토리얼 텍스트
    public TextMeshProUGUI countdownText; // 카운트다운 텍스트

    private string[] tutorialSteps = new string[]
    {
        "드론 농사 짓기 체험에 오신\n 여러분을 환영합니다!",
        "왼쪽 조이스틱\n 위: 상승  아래: 하강\n 좌: 좌회전  우: 우회전",
        "오른쪽 조이스틱\n 위: 전진  아래: 후진\n  좌: 좌측 이동  우: 우측 이동",
        "왼쪽 컨트롤러의 X 버튼을 누르면\n 드론 시점->3인칭 시점순으로 \n바뀌게 됩니다.",
        "왼쪽 그립 버튼을 눌러 \n먼저 씨를 뿌리고",
        "오른쪽 그립 버튼을 눌러\n 물을 뿌립니다.",
        "이제 본격적으로 토마토 키우기를\n 시작해보겠습니다!",
        "120초 안에 토마토를\n 수확해주세요!"
    };

    private void Start()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true); // 튜토리얼 패널 활성화
            StartCoroutine(DisplayTutorialSteps());
        }
    }

    private IEnumerator DisplayTutorialSteps()
    {
        foreach (string step in tutorialSteps)
        {
            tutorialText.text = step;
            yield return new WaitForSeconds(5f); // 각 단계별로 5초 동안 대기
        }

        tutorialPanel.SetActive(false); // 모든 단계가 끝난 후 패널 비활성화
        StartCoroutine(StartCountdown(60)); // 튜토리얼 단계가 끝난 후 120초 카운트다운 시작
    }

    private IEnumerator StartCountdown(int seconds)
    {
        int remainingTime = seconds;
        while (remainingTime > 0)
        {
            countdownText.text = remainingTime.ToString(); // 남은 시간을 카운트다운 텍스트로 표시
            yield return new WaitForSeconds(1f); // 1초 대기
            remainingTime--;
        }

        countdownText.text = "Game Over";
        countdownText.color = Color.red;
        yield return new WaitForSeconds(1f); // 1초 대기

        DroneUIPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        tutorialText.text = "수고하셨습니다.\n토마토 키우기를 완료했습니다!";
    }
}
