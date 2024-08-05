using System.Collections;
using UnityEngine;
using TMPro;

public class FarmTutorialPanel : MonoBehaviour
{
    public GameObject tPanel; // Ʃ�丮�� �г�
    public TextMeshProUGUI tText; // Ʃ�丮�� �ؽ�Ʈ

    private string[] Steps = new string[]
    {
        "��� ���� �Ʒÿ� ���� �������� ȯ���մϴ�!",
        "���� ���̽�ƽ\n ��: ���, �Ʒ�: �ϰ�\n ��: ��ȸ��, ��: ��ȸ��",
        "������ ���̽�ƽ\n ��: ����, �Ʒ�: ����\n ��: ���� �̵�, ��: ���� �̵�",
        "���� ��Ʈ�ѷ��� X ��ư�� ������ ��� ī�޶� �������� �ٲ�� �˴ϴ�.",
        "���� ���������� ��� ������ �����ϰڽ��ϴ�!",
        "8���� ������ �ڽ��� ������ �̼� �����Դϴ�."
    };

    private void Start()
    {
        if (tPanel != null)
        {
            tPanel.SetActive(true); // Ʃ�丮�� �г� Ȱ��ȭ
            StartCoroutine(TutorialSteps());
        }
    }

    private IEnumerator TutorialSteps()
    {
        foreach (string step in Steps)
        {
            tText.text = step;
            yield return new WaitForSeconds(5f); // �� �ܰ躰�� 5�� ���� ���
        }

        tPanel.SetActive(false); // ��� �ܰ谡 ���� �� �г� ��Ȱ��ȭ
        tText.text = "��� ����Ʈ�� �޼��߽��ϴ�!";
    }
}
