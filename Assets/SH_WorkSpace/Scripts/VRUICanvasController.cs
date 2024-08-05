using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUICanvasController : MonoBehaviour
{
	public Canvas vrCanvas; // VR Canvas

	void Start()
	{
		if (vrCanvas != null)
		{
			vrCanvas.gameObject.SetActive(false); // 처음에는 비활성화
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			// F 키를 눌렀을 때 활성화 상태를 토글
			vrCanvas.gameObject.SetActive(!vrCanvas.gameObject.activeSelf);
		}
	}
}
