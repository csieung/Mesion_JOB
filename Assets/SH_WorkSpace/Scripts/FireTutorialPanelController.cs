using System.Collections;
using UnityEngine;
using TMPro;

public class FireTutorialPanelController : MonoBehaviour
{
	public GameObject tutorialPanel; // Ʃ�丮�� �г�
	public TextMeshProUGUI tutorialText; // Ʃ�丮�� �ؽ�Ʈ

	private string[] tutorialSteps = new string[]
	{
		"���� ����� �����Ͽ� \nȭ�� ������ �� ���Դϴ�.",
		"������ ��Ʈ�ѷ��� �׸� ��ư�� \n������ ���� �л�˴ϴ�.",
		"���� �������� �� �ǹ���\n �ִ� ���� ���ø� �˴ϴ�.",
	};

	private void Start()
	{
		if (tutorialPanel != null)
		{
			tutorialPanel.SetActive(true); // Ʃ�丮�� �г� Ȱ��ȭ
			StartCoroutine(DisplayTutorialSteps());
		}
	}

	private IEnumerator DisplayTutorialSteps()
	{
		foreach (string step in tutorialSteps)
		{
			tutorialText.text = step;
			yield return new WaitForSeconds(5f); // �� �ܰ躰�� 5�� ���� ���
		}

		tutorialPanel.SetActive(false); // ��� �ܰ谡 ���� �� �г� ��Ȱ��ȭ
		tutorialText.text = "ȭ�� ���п� �����ϼ̽��ϴ�.";
	}
}
