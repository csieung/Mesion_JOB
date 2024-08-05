using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FireController : MonoBehaviour
{
	public float health = 100f; // ���� �ʱ� ü��
	public float extinguishRate = 10f; // ����￡ ���� �� ������ ü�·�
	public float extinguishTime = 2f; // ���� ������ ����������� �ð�
	
	public string nextSceneName; // ���� �� �̸�
	public GameObject tutorialPanel; // Ʃ�丮�� �г�
	
	private bool isExtinguishing = false;
	private ParticleSystem fireParticle;

	void Start()
	{
		fireParticle = GetComponent<ParticleSystem>();
		if (fireParticle == null)
		{
			Debug.LogError("Particle System not found on this GameObject");
		}

		if (tutorialPanel != null)
		{
			tutorialPanel.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
		}
	}

	public void ApplyWater()
	{
		if (!isExtinguishing)
		{
			health -= extinguishRate;
			Debug.Log("Fire health: " + health);

			if (health <= 0f)
			{
				StartCoroutine(Extinguish());
			}
		}
	}

	private IEnumerator Extinguish()
	{
		isExtinguishing = true;
		float elapsedTime = 0f;

		while (elapsedTime < extinguishTime)
		{
			float t = elapsedTime / extinguishTime;
			fireParticle.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		fireParticle.Stop();
		Debug.Log("Fire Extinguished");

		if (SceneManager.GetActiveScene().name == "Fire_Training")
		{
			if (tutorialPanel != null)
			{
				tutorialPanel.SetActive(true);
			}

			yield return new WaitForSeconds(3f); // 3�� ���� ���
			SceneManager.LoadScene(nextSceneName); // ���� ������ ��ȯ
		}
	}
}
