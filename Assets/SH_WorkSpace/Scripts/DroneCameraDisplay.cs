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
			droneCameraFeed.gameObject.SetActive(false); // 처음에는 비활성화
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			// F 키를 눌렀을 때 활성화 상태를 토글
			droneCameraFeed.gameObject.SetActive(!droneCameraFeed.gameObject.activeSelf);
		}
	}
}
