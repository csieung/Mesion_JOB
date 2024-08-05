using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroplet : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Fire"))
		{
			FireController fireController = collision.gameObject.GetComponent<FireController>();
			if (fireController != null)
			{
				fireController.ApplyWater();
				Destroy(gameObject); // ������� �浹 �� �ı��˴ϴ�.
			}
		}
	}
}
