using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialPanel; // Ʃ�丮�� �г�
    public GameObject DroneUIPanel; // Ʃ�丮�� �г�
    public TextMeshProUGUI tutorialText; // Ʃ�丮�� �ؽ�Ʈ
    public TextMeshProUGUI countdownText; // ī��Ʈ�ٿ� �ؽ�Ʈ

    private string[] tutorialSteps = new string[]
    {
        "��� ��� ���� ü�迡 ����\n �������� ȯ���մϴ�!",
        "���� ���̽�ƽ\n ��: ���  �Ʒ�: �ϰ�\n ��: ��ȸ��  ��: ��ȸ��",
        "������ ���̽�ƽ\n ��: ����  �Ʒ�: ����\n  ��: ���� �̵�  ��: ���� �̵�",
        "���� ��Ʈ�ѷ��� X ��ư�� ������\n ��� ����->3��Ī ���������� \n�ٲ�� �˴ϴ�.",
        "���� �׸� ��ư�� ���� \n���� ���� �Ѹ���",
        "������ �׸� ��ư�� ����\n ���� �Ѹ��ϴ�.",
        "���� ���������� �丶�� Ű��⸦\n �����غ��ڽ��ϴ�!",
        "120�� �ȿ� �丶�並\n ��Ȯ���ּ���!"
    };

    private void Start()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true); // Ʃ�丮�� �г� Ȱ��ȭ
            StartCoroutine(DisplayTutorialSteps());
        }
    }

    private IEnumerator DisplayTutorialSteps()
    {
        foreach (string step in tutorialSteps)
        {
            tutorialText.text = step;
            yield return new WaitForSeconds(5f); // �� �ܰ躰�� 5�� ���� ���
        }

        tutorialPanel.SetActive(false); // ��� �ܰ谡 ���� �� �г� ��Ȱ��ȭ
        StartCoroutine(StartCountdown(60)); // Ʃ�丮�� �ܰ谡 ���� �� 120�� ī��Ʈ�ٿ� ����
    }

    private IEnumerator StartCountdown(int seconds)
    {
        int remainingTime = seconds;
        while (remainingTime > 0)
        {
            countdownText.text = remainingTime.ToString(); // ���� �ð��� ī��Ʈ�ٿ� �ؽ�Ʈ�� ǥ��
            yield return new WaitForSeconds(1f); // 1�� ���
            remainingTime--;
        }

        countdownText.text = "Game Over";
        countdownText.color = Color.red;
        yield return new WaitForSeconds(1f); // 1�� ���

        DroneUIPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        tutorialText.text = "�����ϼ̽��ϴ�.\n�丶�� Ű��⸦ �Ϸ��߽��ϴ�!";
    }
}
