using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterSprayController : MonoBehaviour
{
	public GameObject waterDropletPrefab; // ����� ������
	public Transform sprayOrigin; // �� �л� ���� ��ġ
	public float sprayForce = 10f; // �� �л� ��
	public float dropletLifetime = 2f; // ����� ���� �ð�
	public int dropletsPerSecond = 10; // �ʴ� ������ ����� ����
	public float sprayAngle = 15f; // ����� �л� �ִ� ����

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
			for (int i = 0; i < 3; i++) // �� ���� ���� ����� ����
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
