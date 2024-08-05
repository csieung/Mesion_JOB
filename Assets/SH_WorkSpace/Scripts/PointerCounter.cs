using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointerCounter : MonoBehaviour
{
	public TextMeshProUGUI counterText; // �ؽ�Ʈ UI ���
	private int pointerCount = 0;

	void Start()
	{
		UpdateCounterText();
	}

	// �����͸� ����� �� ȣ��Ǵ� �޼���
	public void OnPointerHit()
	{
		pointerCount++;
		UpdateCounterText();
	}

	// �ؽ�Ʈ UI ��Ҹ� ������Ʈ�ϴ� �޼���
	private void UpdateCounterText()
	{
		counterText.text = "������ ��: " + pointerCount.ToString();
	}
}
