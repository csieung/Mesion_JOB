using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRCotrollerTest : MonoBehaviour
{
	public InputActionProperty leftMoveAction;
	public InputActionProperty rightMoveAction;
	public InputActionProperty leftRotateAction;
	public InputActionProperty rightRotateAction;
	public InputActionProperty triggerAction;
	public InputActionProperty gripAction;

	void Update()
	{
		TestLeftController();
		TestRightController();
	}

	void TestLeftController()
	{
		Vector2 leftMoveValue = leftMoveAction.action.ReadValue<Vector2>();
		Vector2 leftRotateValue = leftRotateAction.action.ReadValue<Vector2>();
		bool triggerPressed = triggerAction.action.IsPressed();
		bool gripPressed = gripAction.action.IsPressed();

		Debug.Log($"Left Controller - Move: {leftMoveValue}, Rotate: {leftRotateValue}, Trigger: {triggerPressed}, Grip: {gripPressed}");
	}

	void TestRightController()
	{
		Vector2 rightMoveValue = rightMoveAction.action.ReadValue<Vector2>();
		Vector2 rightRotateValue = rightRotateAction.action.ReadValue<Vector2>();
		bool triggerPressed = triggerAction.action.IsPressed();
		bool gripPressed = gripAction.action.IsPressed();

		Debug.Log($"Right Controller - Move: {rightMoveValue}, Rotate: {rightRotateValue}, Trigger: {triggerPressed}, Grip: {gripPressed}");
	}
}
