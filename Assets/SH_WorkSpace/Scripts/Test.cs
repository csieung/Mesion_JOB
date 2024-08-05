using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public float moveSpeed = 100.0f;
    public float ascendSpeed = 100.0f;
    public float rotationSpeed = 100.0f;
    public Camera droneCamera;
    public Camera controllerCamera;

    public InputActionProperty leftMoveAction;
    public InputActionProperty rightMoveAction;
    public InputActionProperty leftRotateAction;
    public InputActionProperty rightRotateAction;
    public InputActionProperty ascendAction;
    public InputActionProperty descendAction;
    public InputActionProperty switchCameraAction;

    public Rigidbody rb;
    private bool isDroneView = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SwitchCameraView(isDroneView);
    }

	void Update()
	{
		HandleMovement();
		HandleRotation();
		HandleCameraSwitch();
	}

	void HandleMovement()
	{
		Vector2 leftMoveValue = leftMoveAction.action.ReadValue<Vector2>();
		Vector2 rightMoveValue = rightMoveAction.action.ReadValue<Vector2>();

		float moveForwardBackward = rightMoveValue.y * moveSpeed;
		float moveLeftRight = rightMoveValue.x * moveSpeed;
		float moveUpDown = 0f;

		if (ascendAction.action.IsPressed())
		{
			moveUpDown = ascendSpeed;
		}
		else if (descendAction.action.IsPressed())
		{
			moveUpDown = -ascendSpeed;
		}

		Vector3 movement = new Vector3(moveLeftRight, moveUpDown, moveForwardBackward);
		Vector3 movementDirection = transform.TransformDirection(movement);

		rb.velocity = movementDirection * Time.deltaTime;
	}

	void HandleRotation()
	{
		Vector2 leftRotateValue = leftRotateAction.action.ReadValue<Vector2>();

		float rotateYaw = leftRotateValue.x * rotationSpeed * Time.deltaTime;

		transform.Rotate(0f, rotateYaw, 0f);
	}

	void HandleCameraSwitch()
	{
		if (switchCameraAction.action.WasPressedThisFrame())
		{
			isDroneView = !isDroneView;
			SwitchCameraView(isDroneView);
		}
	}

	void SwitchCameraView(bool isDroneView)
	{
		droneCamera.gameObject.SetActive(isDroneView);
		controllerCamera.gameObject.SetActive(!isDroneView);
	}
}
