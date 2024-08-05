using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using TMPro;
using LazyFollow = UnityEngine.XR.Interaction.Toolkit.UI.LazyFollow;

// Goal 구조체 정의
public struct Goal
{
    public GoalManager.OnboardingGoals CurrentGoal; // 현재 목표
    public bool Completed; // 완료 여부

    public Goal(GoalManager.OnboardingGoals goal)
    {
        CurrentGoal = goal;
        Completed = false; // 초기 상태는 완료되지 않음
    }
}

public class GoalManager : MonoBehaviour
{
    // 온보딩 목표를 정의하는 열거형
    public enum OnboardingGoals
    {
        Empty,
        FindSurfaces,
        SelectWorld,
        EnterWorld,
        //TapSurface,
    }

    Queue<Goal> m_OnboardingGoals; // 온보딩 목표를 담는 큐
    Goal m_CurrentGoal; // 현재 목표
    bool m_AllGoalsFinished; // 모든 목표가 완료되었는지 여부
    int m_SurfacesTapped; // 탭된 표면의 수
    int m_CurrentGoalIndex = 0; // 현재 목표 인덱스

    [Serializable]
    class Step
    {
        [SerializeField]
        public GameObject stepObject; // 단계 객체

        //[SerializeField]
        //public string buttonText; // 버튼 텍스트

        //public bool includeSkipButton; // 건너뛰기 버튼 포함 여부
    }

    [SerializeField]
    List<Step> m_StepList = new List<Step>(); // 단계 리스트

    [SerializeField]
    public TextMeshProUGUI m_StepButtonTextField; // 단계 버튼 텍스트 필드

    [SerializeField]
    public GameObject m_StartNewButton; // 새출발 버튼

    [SerializeField]
    public GameObject m_ExitButton; // 종료 버튼

    [SerializeField]
    public GameObject m_ContinueButton; // 계속하기 버튼

    [SerializeField]
    public GameObject m_SkipButton; // 건너뛰기 버튼

    [SerializeField]
    public GameObject m_QuitButton; // 강제종료

    [SerializeField]
    GameObject m_LearnButton; // 학습 버튼

    [SerializeField]
    GameObject m_LearnModal; // 학습 모달

    [SerializeField]
    Button m_LearnModalButton; // 학습 모달 버튼

    [SerializeField]
    GameObject m_CoachingUIParent; // 코칭 UI 부모 객체

    [SerializeField]
    FadeMaterial m_FadeMaterial; // 페이드 소재

    [SerializeField]
    Toggle m_PassthroughToggle; // 패스스루 토글

    [SerializeField]
    LazyFollow m_GoalPanelLazyFollow; // 목표 패널 LazyFollow

    //[SerializeField]
    //GameObject m_TapTooltip; // 탭 툴팁

    //[SerializeField]
    //GameObject m_VideoPlayer; // 비디오 플레이어

    //[SerializeField]
    //Toggle m_VideoPlayerToggle; // 비디오 플레이어 토글

    [SerializeField]
    ARPlaneManager m_ARPlaneManager; // AR 평면 관리자

    //[SerializeField]
    //ObjectSpawner m_ObjectSpawner; // 객체 생성기

    //const int k_NumberOfSurfacesTappedToCompleteGoal = 1; // 목표 완료를 위한 탭 횟수
    Vector3 m_TargetOffset = new Vector3(-.5f, -.25f, 1.5f); // 목표 오프셋

    void Start()
    {
        // 온보딩 목표 초기화
        m_OnboardingGoals = new Queue<Goal>();
        var welcomeGoal = new Goal(OnboardingGoals.Empty);
        var findSurfaceGoal = new Goal(OnboardingGoals.FindSurfaces);
        var selectWorldGoal = new Goal(OnboardingGoals.SelectWorld);
        //var tapSurfaceGoal = new Goal(OnboardingGoals.TapSurface);
        var enterWorldGoal = new Goal(OnboardingGoals.EnterWorld);

        m_OnboardingGoals.Enqueue(welcomeGoal);
        m_OnboardingGoals.Enqueue(findSurfaceGoal);
        //m_OnboardingGoals.Enqueue(tapSurfaceGoal);
        m_OnboardingGoals.Enqueue(selectWorldGoal);
        m_OnboardingGoals.Enqueue(enterWorldGoal);

        //목표 하나 빼기
        m_CurrentGoal = m_OnboardingGoals.Dequeue();
        //초기화
        //if (m_TapTooltip != null)
        //    m_TapTooltip.SetActive(false);

        //if (m_VideoPlayer != null)
        //{
        //    m_VideoPlayer.SetActive(false);

        //    if (m_VideoPlayerToggle != null)
        //        m_VideoPlayerToggle.isOn = false;
        //}

        if (m_FadeMaterial != null)
        {
            m_FadeMaterial.FadeSkybox(false);

            if (m_PassthroughToggle != null)
                m_PassthroughToggle.isOn = false;
        }

        if (m_LearnButton != null)
        {
            m_LearnButton.GetComponent<Button>().onClick.AddListener(OpenModal); ;
            m_LearnButton.SetActive(false);
        }

        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.zero;
        }

        if (m_LearnModalButton != null)
        {
            m_LearnModalButton.onClick.AddListener(CloseModal);
        }

        //        if (m_ObjectSpawner == null)
        //        {
        //#if UNITY_2023_1_OR_NEWER
        //            m_ObjectSpawner = FindAnyObjectByType<ObjectSpawner>();
        //#else
        //            m_ObjectSpawner = FindObjectOfType<ObjectSpawner>();
        //#endif
        //        }
    }

    // 모달 열기
    void OpenModal()
    {
        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.one;
        }
    }

    // 모달 닫기
    void CloseModal()
    {
        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.zero;
        }
    }
    void Update()
    {
        if (!m_AllGoalsFinished)
        {
            ProcessGoals();
        }

        // 디버그 입력 (유니티 에디터)
#if UNITY_EDITOR
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CompleteGoal();
        }
#endif
    }

    // 목표 처리
    void ProcessGoals()
    {
        if (!m_CurrentGoal.Completed)
        {
            switch (m_CurrentGoal.CurrentGoal)
            {
                case OnboardingGoals.Empty:
                    m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
                    break;
                case OnboardingGoals.FindSurfaces:
                    m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
                    break;
                case OnboardingGoals.SelectWorld:
                    //if (m_TapTooltip != null)
                    //{
                    //    m_TapTooltip.SetActive(true);
                    //}
                    m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
                    break;
                case OnboardingGoals.EnterWorld:
                    m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.None;
                    break;
            }
        }
    }

    // 목표 완료
    void CompleteGoal()
    {
        if (m_CurrentGoal.CurrentGoal == OnboardingGoals.EnterWorld)
            //m_ObjectSpawner.objectSpawned -= OnObjectSpawned;

            // 다음 목표 설정 전에 툴팁 비활성화
            //DisableTooltips();

            m_CurrentGoal.Completed = true;
        m_CurrentGoalIndex++;
        if (m_OnboardingGoals.Count > 0)
        {
            // 큐에서 다음 목표를 가져옴
            m_CurrentGoal = m_OnboardingGoals.Dequeue();

            // 이전 단계의 객체를 비활성화
            m_StepList[m_CurrentGoalIndex - 1].stepObject.SetActive(false);

            // 현재 단계의 객체를 활성화
            m_StepList[m_CurrentGoalIndex].stepObject.SetActive(true);

            // 버튼 텍스트를 현재 단계의 버튼 텍스트로 설정
            //m_StepButtonTextField.text = m_StepList[m_CurrentGoalIndex].buttonText;

            // 현재 단계에 스킵 버튼을 포함하는지 여부에 따라 스킵 버튼을 활성화 또는 비활성화
            //m_SkipButton.SetActive(m_StepList[m_CurrentGoalIndex].includeSkipButton);
        }
        else
        {
            // 모든 목표가 완료된 경우
            m_AllGoalsFinished = true;
            ForceEndAllGoals();
        }

        // 현재 목표가 OnboardingGoals.FindSurfaces인 경우
        if (m_CurrentGoal.CurrentGoal == OnboardingGoals.FindSurfaces)
        {
            if (m_FadeMaterial != null)
                m_FadeMaterial.FadeSkybox(true);

            if (m_PassthroughToggle != null)
                m_PassthroughToggle.isOn = true;

            if (m_LearnButton != null)
            {
                m_LearnButton.SetActive(true);
            }

            StartCoroutine(TurnOnPlanes());
        }
        //else if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface)
        //{
        //    if (m_LearnButton != null)
        //    {
        //        m_LearnButton.SetActive(false);
        //    }
        //    m_SurfacesTapped = 0;
        //    //m_ObjectSpawner.objectSpawned += OnObjectSpawned;
        //}
    }

    // 평면 활성화 코루틴
    public IEnumerator TurnOnPlanes()
    {
        yield return new WaitForSeconds(1f);
        m_ARPlaneManager.enabled = true;
    }

    //// 툴팁 비활성화
    //void DisableTooltips()
    //{
    //    if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface)
    //    {
    //        if (m_TapTooltip != null)
    //        {
    //            m_TapTooltip.SetActive(false);
    //        }
    //    }
    //}

    // 목표 강제 완료
    public void ForceCompleteGoal()
    {
        CompleteGoal();
    }

    // 모든 목표 강제 종료
    public void ForceEndAllGoals()
    {
        m_CoachingUIParent.transform.localScale = Vector3.zero;

        //TurnOnVideoPlayer();

        //if (m_VideoPlayerToggle != null)
        //    m_VideoPlayerToggle.isOn = true;

        if (m_FadeMaterial != null)
        {
            m_FadeMaterial.FadeSkybox(true);

            if (m_PassthroughToggle != null)
                m_PassthroughToggle.isOn = true;
        }

        if (m_LearnButton != null)
        {
            m_LearnButton.SetActive(false);
        }

        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.zero;
        }

        StartCoroutine(TurnOnPlanes());
    }

    // 코칭 리셋
    public void ResetCoaching()
    {
        m_CoachingUIParent.transform.localScale = Vector3.one;

        m_OnboardingGoals.Clear();
        m_OnboardingGoals = new Queue<Goal>();
        var welcomeGoal = new Goal(OnboardingGoals.Empty);
        var findSurfaceGoal = new Goal(OnboardingGoals.FindSurfaces);
        var selectWorldGoal = new Goal(OnboardingGoals.SelectWorld);
        //var tapSurfaceGoal = new Goal(OnboardingGoals.TapSurface);
        var enterWorldGoal = new Goal(OnboardingGoals.EnterWorld);

        m_OnboardingGoals.Enqueue(welcomeGoal);
        m_OnboardingGoals.Enqueue(findSurfaceGoal);
        m_OnboardingGoals.Enqueue(selectWorldGoal);
        m_OnboardingGoals.Enqueue(enterWorldGoal);

        //for (int i = 0; i < m_StepList.Count; i++)
        //{
        //    if (i == 0)
        //    {
        //        m_StepList[i].stepObject.SetActive(true);
        //        m_SkipButton.SetActive(m_StepList[i].includeSkipButton);
        //        m_StepButtonTextField.text = m_StepList[i].buttonText;
        //    }
        //    else
        //    {
        //        m_StepList[i].stepObject.SetActive(false);
        //    }
        //}

        m_CurrentGoal = m_OnboardingGoals.Dequeue();
        m_AllGoalsFinished = false;

        //if (m_TapTooltip != null)
        //    m_TapTooltip.SetActive(false);

        if (m_LearnButton != null)
        {
            m_LearnButton.SetActive(false);
        }

        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.zero;
        }

        m_CurrentGoalIndex = 0;
    }
    // 프로그램 종료
    public void ForceQuitApplication()
    {
        // 애플리케이션 종료 로직
        Application.Quit();

#if UNITY_EDITOR
        // Unity 에디터에서 실행 중인 경우 플레이 모드를 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    // 객체 생성 시 호출되는 이벤트 핸들러
    //void OnObjectSpawned(GameObject spawnedObject)
    //{
    //    m_SurfacesTapped++;
    //    if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface && m_SurfacesTapped >= k_NumberOfSurfacesTappedToCompleteGoal)
    //    {
    //        CompleteGoal();
    //        m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
    //    }
    //}

    // 비디오 플레이어 토글
    //public void TooglePlayer(bool visibility)
    //{
    //    if (visibility)
    //    {
    //        TurnOnVideoPlayer();
    //    }
    //    else
    //    {
    //        if (m_VideoPlayer.activeSelf)
    //        {
    //            m_VideoPlayer.SetActive(false);
    //            if (m_VideoPlayerToggle.isOn)
    //                m_VideoPlayerToggle.isOn = false;
    //        }
    //    }
    //}

    // 비디오 플레이어 활성화
    //void TurnOnVideoPlayer()
    //{
    //    if (m_VideoPlayer.activeSelf)
    //        return;

    //    var follow = m_VideoPlayer.GetComponent<LazyFollow>();
    //    if (follow != null)
    //        follow.rotationFollowMode = LazyFollow.RotationFollowMode.None;

    //    m_VideoPlayer.SetActive(false);
    //    var target = Camera.main.transform;
    //    var targetRotation = target.rotation;
    //    var newTransform = target;
    //    var targetEuler = targetRotation.eulerAngles;
    //    targetRotation = Quaternion.Euler
    //    (
    //        0f,
    //        targetEuler.y,
    //        targetEuler.z
    //    );

    //    newTransform.rotation = targetRotation;
    //    var targetPosition = target.position + newTransform.TransformVector(m_TargetOffset);
    //    m_VideoPlayer.transform.position = targetPosition;

    //    var forward = target.position - m_VideoPlayer.transform.position;
    //    var targetPlayerRotation = forward.sqrMagnitude > float.Epsilon ? Quaternion.LookRotation(forward, Vector3.up) : Quaternion.identity;
    //    targetPlayerRotation *= Quaternion.Euler(new Vector3(0f, 180f, 0f));
    //    var targetPlayerEuler = targetPlayerRotation.eulerAngles;
    //    var currentEuler = m_VideoPlayer.transform.rotation.eulerAngles;
    //    targetPlayerRotation = Quaternion.Euler
    //    (
    //        currentEuler.x,
    //        targetPlayerEuler.y,
    //        currentEuler.z
    //    );

    //    m_VideoPlayer.transform.rotation = targetPlayerRotation;
    //    m_VideoPlayer.SetActive(true);
    //    if (follow != null)
    //        follow.rotationFollowMode = LazyFollow.RotationFollowMode.LookAtWithWorldUp;
    //}
}
