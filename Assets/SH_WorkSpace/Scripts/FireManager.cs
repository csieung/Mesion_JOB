using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FireManager : MonoBehaviour
{
	public GameObject firePrefab;
	public Transform[] fireLocations;
	public float fireInterval = 5f;
	public GameObject successMessage; // 성공 메시지 텍스트

	private HashSet<Transform> activeFireLocations = new HashSet<Transform>();

	int cnt = 0;

	void Start()
	{
		InvokeRepeating("ActivateRandomFire", fireInterval, fireInterval);
	}

	void ActivateRandomFire()
	{
		if (activeFireLocations.Count >= fireLocations.Length)
		{
			return;
		}

		Transform fireLocation = null;
		do
		{
			int randomIndex = Random.Range(0, fireLocations.Length);
			fireLocation = fireLocations[randomIndex];
		} while (activeFireLocations.Contains(fireLocation));

		Instantiate(firePrefab, fireLocation.position, fireLocation.rotation);
		activeFireLocations.Add(fireLocation);
		cnt++;

		HandleFireExtinguished(cnt);
	}

	void HandleFireExtinguished(int cnt)
	{
		// 모든 불이 꺼졌는지 확인
		if (fireLocations.Length == cnt)
		{
			ShowSuccessMessage();
		}
	}

	void ShowSuccessMessage()
	{
		if (successMessage != null)
		{
			successMessage.SetActive(true);
		}
		Debug.Log("All fires extinguished! Success!");
	}
}
