using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DroneStatus : MonoBehaviour
{
	public Slider batterySlider;
	public Slider waterTankSlider;
	public TextMeshProUGUI timerText;

	private float batteryLevel = 500.0f;
	private float waterLevel = 100.0f;
	private float timer = 300.0f;

	private bool isGameOver = false;
	public GameObject GameOverPanel;

	[SerializeField]
	private InputActionReference useWaterAction;

	private void OnEnable()
	{
		useWaterAction.action.Enable();
	}

	private void OnDisable()
	{
		useWaterAction.action.Disable();
	}

	// Start is called before the first frame update
	void Start()
	{
		UpdateUI();
	}

	// Update is called once per frame
	void Update()
	{
		if (!isGameOver)
		{
			UpdateStatus();
			CheckGameOver();
			UpdateUI();
		}
	}

	void UpdateStatus()
	{
		if (batteryLevel > 0)
		{
			batteryLevel -= Time.deltaTime;
		}
		if (useWaterAction.action.ReadValue<float>() > 0 && waterLevel > 0)
		{
			waterLevel -= Time.deltaTime * 10;
		}

		if (timer > 0)
		{
			timer -= Time.deltaTime;
		}
	}

	void CheckGameOver()
	{
		if (batteryLevel <= 0 || waterLevel <= 0 || timer <= 0)
		{
			isGameOver = true;
			GameOver();
		}
	}

	void GameOver()
	{
		// 게임 오버 로직
		Debug.Log("Game Over!");
		GameOverPanel.SetActive(true); // 게임 오버 씬으로 전환
	}

	void UpdateUI()
	{
		batterySlider.value = batteryLevel;
		waterTankSlider.value = waterLevel;

		int minutes = Mathf.FloorToInt(timer / 60f);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);
		timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
	}
}
