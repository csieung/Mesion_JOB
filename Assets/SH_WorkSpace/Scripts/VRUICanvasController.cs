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
			vrCanvas.gameObject.SetActive(false); // ó������ ��Ȱ��ȭ
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			// F Ű�� ������ �� Ȱ��ȭ ���¸� ���
			vrCanvas.gameObject.SetActive(!vrCanvas.gameObject.activeSelf);
		}
	}
}
