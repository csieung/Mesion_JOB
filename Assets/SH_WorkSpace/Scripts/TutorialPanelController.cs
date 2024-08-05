using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialPanelController : MonoBehaviour
{
	public GameObject tutorialPanel; // 튜토리얼 패널
	public TextMeshProUGUI tutorialText; // 튜토리얼 텍스트
	public PointController pointController; // 포인트 컨트롤러 참조

	private string[] tutorialSteps = new string[]
	{
		"드론 조종 훈련에 오신\n 여러분을 환영합니다!",
		"왼쪽 조이스틱\n 위: 상승, 아래: 하강\n 좌: 좌회전, 우: 우회전",
		"오른쪽 조이스틱\n 위: 전진, 아래: 후진\n 좌: 좌측 이동, 우: 우측 이동",
		"왼쪽 컨트롤러의 X 버튼을 누르면\n 드론 카메라 시점으로 바뀌게 됩니다.",
		"이제 본격적으로 \n드론 조작을 연습하겠습니다!",
		"8개의 빨간색 박스를\n 찍으면 미션 성공입니다."
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
		tutorialText.text = "모든 포인트를 달성했습니다!";
		pointController.EnablePoints(); // 튜토리얼이 끝난 후 포인트 활성화
	}
}
