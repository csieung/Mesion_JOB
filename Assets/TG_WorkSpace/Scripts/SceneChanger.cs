using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    // 전환할 씬의 이름
    public string sceneName;

    // 버튼 참조
    public Button changeSceneButton;

    void Start()
    {
        if (changeSceneButton != null)
        {
            // 버튼 클릭 이벤트에 메서드 등록
            changeSceneButton.onClick.AddListener(OnChangeSceneButtonClicked);
        }
    }

    // 버튼 클릭 시 호출될 메서드
    void OnChangeSceneButtonClicked()
    {
        // 씬 전환
        SceneManager.LoadScene(sceneName);
    }
}
