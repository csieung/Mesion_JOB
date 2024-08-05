using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointController : MonoBehaviour
{
	public GameObject[] points;
	public GameObject tutorialPanel; // Ʃ�丮�� �г�
	public TextMeshProUGUI tutorialText; // Ʃ�丮�� �ؽ�Ʈ
	public string nextSceneName; // ���� �� �̸�
	private int currentPointIndex = 0;
	private PointerCounter pointerCounter; // ������ ī���� ��ũ��Ʈ ����
	private bool pointsEnabled = false; // ������ Ȱ��ȭ ����

	void Start()
	{
		pointerCounter = FindObjectOfType<PointerCounter>(); // PointerCounter ��ũ��Ʈ ã��

		for (int i = 0; i < points.Length; i++)
		{
			points[i].SetActive(false);
		}

		if (points.Length > 0)
		{
			points[0].SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
		}

		if (tutorialPanel != null)
		{
			tutorialPanel.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
		}
	}

	public void EnablePoints()
	{
		pointsEnabled = true;
		if (points.Length > 0)
		{
			points[0].SetActive(true); // ����Ʈ Ȱ��ȭ
		}
	}

	public void OnPointReached()
	{
		if (!pointsEnabled)
		{
			return; // �����Ͱ� ��Ȱ��ȭ�� ��� ��ȯ
		}

		points[currentPointIndex].SetActive(false);

		currentPointIndex++;

		if (currentPointIndex < points.Length)
		{
			points[currentPointIndex].SetActive(true);
		}
		else
		{
			Debug.Log("��� ����Ʈ�� �޼��߽��ϴ�!");
			StartCoroutine(DisplayCompletionMessage());
		}

		// ������ ī���Ͱ� null�� �ƴ� ��� ī���͸� ������Ʈ�մϴ�.
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
			/*tutorialText.text = "��� ����Ʈ�� �޼��߽��ϴ�!";
			Debug.Log("Ʃ�丮�� �ؽ�Ʈ�� ����Ǿ����ϴ�: " + tutorialText.text); // �α� �߰�*/
			yield return new WaitForSeconds(3f); // 3�� ���� ���
			SceneManager.LoadScene(nextSceneName); // ���� ������ ��ȯ
		}
		else
		{
			Debug.LogError("TutorialPanel �Ǵ� TutorialText�� �������� �ʾҽ��ϴ�!");
		}
	}
}
