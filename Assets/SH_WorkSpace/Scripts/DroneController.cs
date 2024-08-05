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

		// 매 프레임마다 마지막 위치를 업데이트합니다.
		lastPosition = transform.position;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Map") || collision.gameObject.CompareTag("Fire"))
		{
			// 충돌 시 드론을 마지막 위치로 되돌리고 속도를 0으로 설정하여 밀려나는 것을 방지합니다.
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
