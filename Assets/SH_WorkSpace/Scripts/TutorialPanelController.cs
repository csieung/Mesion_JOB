using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialPanelController : MonoBehaviour
{
	public GameObject tutorialPanel; // Ʃ�丮�� �г�
	public TextMeshProUGUI tutorialText; // Ʃ�丮�� �ؽ�Ʈ
	public PointController pointController; // ����Ʈ ��Ʈ�ѷ� ����

	private string[] tutorialSteps = new string[]
	{
		"��� ���� �Ʒÿ� ����\n �������� ȯ���մϴ�!",
		"���� ���̽�ƽ\n ��: ���, �Ʒ�: �ϰ�\n ��: ��ȸ��, ��: ��ȸ��",
		"������ ���̽�ƽ\n ��: ����, �Ʒ�: ����\n ��: ���� �̵�, ��: ���� �̵�",
		"���� ��Ʈ�ѷ��� X ��ư�� ������\n ��� ī�޶� �������� �ٲ�� �˴ϴ�.",
		"���� ���������� \n��� ������ �����ϰڽ��ϴ�!",
		"8���� ������ �ڽ���\n ������ �̼� �����Դϴ�."
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
		tutorialText.text = "��� ����Ʈ�� �޼��߽��ϴ�!";
		pointController.EnablePoints(); // Ʃ�丮���� ���� �� ����Ʈ Ȱ��ȭ
	}
}
