using System.Collections;
using UnityEngine;
using TMPro;

public class FarmTutorialPanel : MonoBehaviour
{
    public GameObject tPanel; // 튜토리얼 패널
    public TextMeshProUGUI tText; // 튜토리얼 텍스트

    private string[] Steps = new string[]
    {
        "드론 조종 훈련에 오신 여러분을 환영합니다!",
        "왼쪽 조이스틱\n 위: 상승, 아래: 하강\n 좌: 좌회전, 우: 우회전",
        "오른쪽 조이스틱\n 위: 전진, 아래: 후진\n 좌: 좌측 이동, 우: 우측 이동",
        "왼쪽 컨트롤러의 X 버튼을 누르면 드론 카메라 시점으로 바뀌게 됩니다.",
        "이제 본격적으로 드론 조작을 연습하겠습니다!",
        "8개의 빨간색 박스를 찍으면 미션 성공입니다."
    };

    private void Start()
    {
        if (tPanel != null)
        {
            tPanel.SetActive(true); // 튜토리얼 패널 활성화
            StartCoroutine(TutorialSteps());
        }
    }

    private IEnumerator TutorialSteps()
    {
        foreach (string step in Steps)
        {
            tText.text = step;
            yield return new WaitForSeconds(5f); // 각 단계별로 5초 동안 대기
        }

        tPanel.SetActive(false); // 모든 단계가 끝난 후 패널 비활성화
        tText.text = "모든 포인트를 달성했습니다!";
    }
}
