using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointController : MonoBehaviour
{
	public GameObject[] points;
	public GameObject tutorialPanel; // 튜토리얼 패널
	public TextMeshProUGUI tutorialText; // 튜토리얼 텍스트
	public string nextSceneName; // 다음 씬 이름
	private int currentPointIndex = 0;
	private PointerCounter pointerCounter; // 포인터 카운터 스크립트 참조
	private bool pointsEnabled = false; // 포인터 활성화 상태

	void Start()
	{
		pointerCounter = FindObjectOfType<PointerCounter>(); // PointerCounter 스크립트 찾기

		for (int i = 0; i < points.Length; i++)
		{
			points[i].SetActive(false);
		}

		if (points.Length > 0)
		{
			points[0].SetActive(false); // 초기에는 비활성화
		}

		if (tutorialPanel != null)
		{
			tutorialPanel.SetActive(false); // 초기에는 비활성화
		}
	}

	public void EnablePoints()
	{
		pointsEnabled = true;
		if (points.Length > 0)
		{
			points[0].SetActive(true); // 포인트 활성화
		}
	}

	public void OnPointReached()
	{
		if (!pointsEnabled)
		{
			return; // 포인터가 비활성화된 경우 반환
		}

		points[currentPointIndex].SetActive(false);

		currentPointIndex++;

		if (currentPointIndex < points.Length)
		{
			points[currentPointIndex].SetActive(true);
		}
		else
		{
			Debug.Log("모든 포인트를 달성했습니다!");
			StartCoroutine(DisplayCompletionMessage());
		}

		// 포인터 카운터가 null이 아닌 경우 카운터를 업데이트합니다.
		if (pointerCounter != null)
		{
			pointerCounter.OnPointerHit();
		}
	}

	private IEnumerator DisplayCompletionMessage()
	{
		if (tutorialPanel != null && tutorialText != null)
		{
			tutorialPanel.SetActive(true);
			/*tutorialText.text = "모든 포인트를 달성했습니다!";
			Debug.Log("튜토리얼 텍스트가 변경되었습니다: " + tutorialText.text); // 로그 추가*/
			yield return new WaitForSeconds(3f); // 3초 동안 대기
			SceneManager.LoadScene(nextSceneName); // 다음 씬으로 전환
		}
		else
		{
			Debug.LogError("TutorialPanel 또는 TutorialText가 설정되지 않았습니다!");
		}
	}
}
