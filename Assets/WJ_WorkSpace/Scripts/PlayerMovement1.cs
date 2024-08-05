using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement1 : MonoBehaviour
{
    public float speed = 5.0f; // 이동 속도
    public float mouseSensitivity = 100.0f; // 마우스 민감도
    public Button[] uiButtons; // 기존 UI 버튼 배열
    public Button[] newUiButtons; // 새로운 UI 버튼 배열
    public GameObject panel; // 새로운 UI 패널
    private float xRotation = 0f;
    public Transform petTransform; // 펫의 Transform

    private bool isPetFollowing = false; // 펫이 플레이어를 바라보는지 여부

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 잠금 상태로 설정
        Cursor.visible = false; // 마우스 커서를 숨김

        // Rigidbody 설정
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true; // 회전 고정
        }

        // 모든 기존 UI 버튼을 초기에는 비활성화 상태로 설정하고 클릭 이벤트 리스너 추가
        for (int i = 0; i < uiButtons.Length; i++)
        {
            if (uiButtons[i] != null)
            {
                uiButtons[i].gameObject.SetActive(false);
                int index = i; // 로컬 변수를 사용하여 클릭 이벤트 리스너에서 사용
                uiButtons[i].onClick.AddListener(() => OnUIButtonClick(index));
            }
        }

        // 모든 새로운 UI 버튼을 초기에는 비활성화 상태로 설정
        foreach (var button in newUiButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
                int index = System.Array.IndexOf(newUiButtons, button);
                if (index == 4) // Assuming newUiButtons[4] is the button in question
                {
                    button.onClick.AddListener(() => StartCoroutine(DeactivateUIAfterDelay(5f)));
                }
            }
        }

        // 패널을 비활성화 상태로 설정
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    void Update()
    {
        CheckPetInteraction(); // 펫과의 상호작용 체크
        RotatePlayer(); // 시야 회전은 항상 작동하게 유지

        if (!isPetFollowing && !AnyButtonActive())
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        // WASD 키 입력 처리
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 이동 벡터 계산
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        movement = movement.normalized * speed * Time.deltaTime;

        // Rigidbody를 사용한 이동
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement);
        }
        else
        {
            // Rigidbody가 없을 경우 기본 이동 방식
            transform.Translate(movement, Space.World);
        }
    }

    void RotatePlayer()
    {
        // 마우스 입력 처리
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 카메라 회전 제한

        // 카메라 X축 회전
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 플레이어 Y축 회전
        transform.Rotate(Vector3.up * mouseX);
    }

    void CheckPetInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Vector3.Distance(transform.position, petTransform.position) < 5f)
            {
                ToggleUIButtons();
                isPetFollowing = !isPetFollowing;

                if (isPetFollowing)
                {
                    petTransform.LookAt(transform);
                    petTransform.GetComponent<PetRoam>().SetFollowingPlayer(true);
                }
                else
                {
                    petTransform.GetComponent<PetRoam>().SetFollowingPlayer(false);
                }
            }
        }
    }

    void ToggleUIButtons()
    {
        if (uiButtons != null && uiButtons.Length > 0)
        {
            bool shouldActivate = !uiButtons[0].gameObject.activeSelf;

            foreach (var button in uiButtons)
            {
                button.gameObject.SetActive(shouldActivate);
            }

            // UI 버튼 활성화 상태에 따라 커서 표시/숨김 전환
            Cursor.visible = shouldActivate;
            Cursor.lockState = shouldActivate ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    bool AnyButtonActive()
    {
        foreach (var button in uiButtons)
        {
            if (button != null && button.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    void OnUIButtonClick(int buttonIndex)
    {
        // 버튼 인덱스에 따라 다른 동작을 수행
        switch (buttonIndex)
        {
            case 0:
                // uiButtons[0] 버튼 클릭 시 새로운 UI 버튼 및 패널 활성화
                ActivateNewUI();
                break;
            case 1:
                // uiButtons[1] 버튼 클릭 시 동작
                AnotherFunction();
                break;
            case 2:
                // uiButtons[2] 버튼 클릭 시 Space 키를 눌렀을 때와 동일한 동작 수행
                PerformSpaceKeyAction();
                break;
            // 필요에 따라 더 많은 case를 추가
            default:
                Debug.Log("버튼 클릭: " + buttonIndex);
                break;
        }

        // 모든 기존 UI 버튼을 비활성화
        foreach (var button in uiButtons)
        {
            button.gameObject.SetActive(false);
        }

        // UI 버튼 비활성화에 따라 커서 숨김 전환 (case 0 제외)
        if (buttonIndex != 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void ActivateNewUI()
    {
        // 모든 새로운 UI 버튼을 활성화
        foreach (var button in newUiButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(true);
            }
        }

        // 패널을 활성화
        if (panel != null)
        {
            panel.SetActive(true);
        }

        // 마우스 커서를 활성화 상태로 유지
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void PerformSpaceKeyAction()
    {
        if (Vector3.Distance(transform.position, petTransform.position) < 5f)
        {
            ToggleUIButtons();
            isPetFollowing = !isPetFollowing;

            if (isPetFollowing)
            {
                petTransform.LookAt(transform);
                petTransform.GetComponent<PetRoam>().SetFollowingPlayer(true);
            }
            else
            {
                petTransform.GetComponent<PetRoam>().SetFollowingPlayer(false);
            }
        }
    }

    void ActivateOtherFunction()
    {
        // 여기에 원하는 기능 추가
        Debug.Log("Other function activated!");
    }

    void AnotherFunction()
    {
        // 여기에 다른 기능 추가
        Debug.Log("Another function activated!");
    }

    IEnumerator DeactivateUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Deactivate all UI elements
        foreach (var button in newUiButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
            }
        }
        if (panel != null)
        {
            panel.SetActive(false);
        }

        // Reset pet's state
        isPetFollowing = false;
        petTransform.GetComponent<PetRoam>().SetFollowingPlayer(false);
    }
}
