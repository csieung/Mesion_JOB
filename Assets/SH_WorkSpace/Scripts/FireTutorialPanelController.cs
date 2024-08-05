using System.Collections;
using UnityEngine;
using TMPro;

public class FireTutorialPanelController : MonoBehaviour
{
	public GameObject tutorialPanel; // 튜토리얼 패널
	public TextMeshProUGUI tutorialText; // 튜토리얼 텍스트

	private string[] tutorialSteps = new string[]
	{
		"이제 드론을 조종하여 \n화재 진압을 할 것입니다.",
		"오른쪽 컨트롤러의 그립 버튼을 \n누르면 물이 분사됩니다.",
		"이제 여러분은 앞 건물에\n 있는 불을 끄시면 됩니다.",
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
		tutorialText.text = "화재 진압에 성공하셨습니다.";
	}
}
