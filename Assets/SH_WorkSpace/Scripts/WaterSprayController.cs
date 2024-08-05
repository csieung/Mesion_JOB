using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterSprayController : MonoBehaviour
{
	public GameObject waterDropletPrefab; // 물방울 프리팹
	public Transform sprayOrigin; // 물 분사 시작 위치
	public float sprayForce = 10f; // 물 분사 힘
	public float dropletLifetime = 2f; // 물방울 생명 시간
	public int dropletsPerSecond = 10; // 초당 생성할 물방울 개수
	public float sprayAngle = 15f; // 물방울 분사 최대 각도

	private bool isSpraying = false;

	DroneStatus droneStatus;

	[SerializeField]
	private InputActionReference sprayWaterAction;

	private void OnEnable()
	{
		sprayWaterAction.action.Enable();
		sprayWaterAction.action.performed += OnSprayWaterPerformed;
		sprayWaterAction.action.canceled += OnSprayWaterCanceled;
	}

	private void OnDisable()
	{
		sprayWaterAction.action.Disable();
		sprayWaterAction.action.performed -= OnSprayWaterPerformed;
		sprayWaterAction.action.canceled -= OnSprayWaterCanceled;
	}

	void Update()
	{
		/*if (Input.GetKeyDown(KeyCode.Space))
		{
			StartSpraying();
		}

		if (Input.GetKeyUp(KeyCode.Space))
		{
			StopSpraying();
		}*/
	}

	private void OnSprayWaterPerformed(InputAction.CallbackContext context)
	{
		StartSpraying();
	}

	private void OnSprayWaterCanceled(InputAction.CallbackContext context)
	{
		StopSpraying();
	}

	private void StartSpraying()
	{
		isSpraying = true;
		StartCoroutine(SprayWater());
	}

	private void StopSpraying()
	{
		isSpraying = false;
		StopCoroutine(SprayWater());
	}

	private IEnumerator SprayWater()
	{
		while (isSpraying)
		{
			for (int i = 0; i < 3; i++) // 한 번에 여러 물방울 생성
			{
				Vector3 randomDirection = GetRandomDirection();
				CreateWaterDroplet(randomDirection);
			}

			yield return new WaitForSeconds(1f / dropletsPerSecond);
		}
	}

	private Vector3 GetRandomDirection()
	{
		float randomAngle = Random.Range(-sprayAngle, sprayAngle);
		Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
		return rotation * sprayOrigin.forward;
	}

	private void CreateWaterDroplet(Vector3 direction)
	{
		GameObject droplet = Instantiate(waterDropletPrefab, sprayOrigin.position, Quaternion.identity);
		Rigidbody rb = droplet.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.AddForce(direction * sprayForce, ForceMode.Impulse);
		}

		Destroy(droplet, dropletLifetime);
	}
}
