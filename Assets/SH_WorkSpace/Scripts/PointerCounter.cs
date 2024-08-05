using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointerCounter : MonoBehaviour
{
	public TextMeshProUGUI counterText; // 텍스트 UI 요소
	private int pointerCount = 0;

	void Start()
	{
		UpdateCounterText();
	}

	// 포인터를 찍었을 때 호출되는 메서드
	public void OnPointerHit()
	{
		pointerCount++;
		UpdateCounterText();
	}

	// 텍스트 UI 요소를 업데이트하는 메서드
	private void UpdateCounterText()
	{
		counterText.text = "포인터 수: " + pointerCount.ToString();
	}
}
