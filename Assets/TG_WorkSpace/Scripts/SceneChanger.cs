using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    // ��ȯ�� ���� �̸�
    public string sceneName;

    // ��ư ����
    public Button changeSceneButton;

    void Start()
    {
        if (changeSceneButton != null)
        {
            // ��ư Ŭ�� �̺�Ʈ�� �޼��� ���
            changeSceneButton.onClick.AddListener(OnChangeSceneButtonClicked);
        }
    }

    // ��ư Ŭ�� �� ȣ��� �޼���
    void OnChangeSceneButtonClicked()
    {
        // �� ��ȯ
        SceneManager.LoadScene(sceneName);
    }
}
