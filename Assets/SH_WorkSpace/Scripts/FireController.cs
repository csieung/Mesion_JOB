using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FireController : MonoBehaviour
{
	public float health = 100f; // 불의 초기 체력
	public float extinguishRate = 10f; // 물방울에 맞을 때 감소할 체력량
	public float extinguishTime = 2f; // 불이 완전히 꺼지기까지의 시간
	
	public string nextSceneName; // 다음 씬 이름
	public GameObject tutorialPanel; // 튜토리얼 패널
	
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
			tutorialPanel.SetActive(false); // 초기에는 비활성화
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

			yield return new WaitForSeconds(3f); // 3초 동안 대기
			SceneManager.LoadScene(nextSceneName); // 다음 씬으로 전환
		}
	}
}
