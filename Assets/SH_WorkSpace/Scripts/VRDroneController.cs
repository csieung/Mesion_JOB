using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRDroneController : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float rotateSpeed = 100f;
	private Rigidbody rb;
	private Vector2 moveInput;
	private float rotateInput;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		// 捞悼 贸府
		Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
		rb.velocity = transform.forward * move.z + transform.right * move.x;

		// 雀傈 贸府
		transform.Rotate(0, rotateInput * rotateSpeed * Time.deltaTime, 0);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}

	public void OnTurn(InputAction.CallbackContext context)
	{
		rotateInput = context.ReadValue<float>();
	}
}