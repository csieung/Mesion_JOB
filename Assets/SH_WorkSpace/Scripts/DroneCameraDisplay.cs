using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneCameraDisplay : MonoBehaviour
{
	public RawImage droneCameraFeed;
	public RenderTexture renderTexture;

	void Start()
	{
		if (droneCameraFeed != null && renderTexture != null)
		{
			droneCameraFeed.texture = renderTexture;
			droneCameraFeed.gameObject.SetActive(false); // ó������ ��Ȱ��ȭ
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			// F Ű�� ������ �� Ȱ��ȭ ���¸� ���
			droneCameraFeed.gameObject.SetActive(!droneCameraFeed.gameObject.activeSelf);
		}
	}
}
