using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DroneController : MonoBehaviour
{
	[SerializeField]
	private InputActionReference droneMovement;
	[SerializeField]
	private InputActionReference droneRotate;
	[SerializeField]
	private InputActionReference toggleCanvasAction;

	public Canvas vrCanvas;

	private Rigidbody rb;
	private Vector3 lastPosition;

	private void OnEnable()
	{
		droneMovement.action.Enable();
		droneRotate.action.Enable();
		toggleCanvasAction.action.Enable();
	}

	private void OnDisable()
	{
		droneMovement.action.Disable();
		droneRotate.action.Disable();
		toggleCanvasAction.action.Disable();
	}

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		lastPosition = transform.position;

		if (vrCanvas != null)
		{
			vrCanvas.gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update()
	{
		//movement
		Vector3 dir = droneMovement.action.ReadValue<Vector3>();
		Vector3 new3dir = new Vector3(-dir.x, dir.z, dir.y);
		gameObject.transform.Translate(new3dir * 5 * Time.deltaTime);

		//rotation
		float rotate = droneRotate.action.ReadValue<float>();
		gameObject.transform.Rotate(Vector3.forward, rotate * 100 * Time.deltaTime);

		//toggle
		if (toggleCanvasAction.action.triggered)
		{
			vrCanvas.gameObject.SetActive(!vrCanvas.gameObject.activeSelf);
		}

		// �� �����Ӹ��� ������ ��ġ�� ������Ʈ�մϴ�.
		lastPosition = transform.position;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Fire"))
		{
			// �浹 �� ����� ������ ��ġ�� �ǵ����� �ӵ��� 0���� �����Ͽ� �з����� ���� �����մϴ�.
			rb.velocity = Vector3.zero;
			transform.position = lastPosition;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Point"))
		{
			other.GetComponentInParent<PointController>().OnPointReached();
		}
	}
}
