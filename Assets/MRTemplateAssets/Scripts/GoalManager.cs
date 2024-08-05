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

// Goal ����ü ����
public struct Goal
{
    public GoalManager.OnboardingGoals CurrentGoal; // ���� ��ǥ
    public bool Completed; // �Ϸ� ����

    public Goal(GoalManager.OnboardingGoals goal)
    {
        CurrentGoal = goal;
        Completed = false; // �ʱ� ���´� �Ϸ���� ����
    }
}

public class GoalManager : MonoBehaviour
{
    // �º��� ��ǥ�� �����ϴ� ������
    public enum OnboardingGoals
    {
        Empty,
        FindSurfaces,
        SelectWorld,
        EnterWorld,
        //TapSurface,
    }

    Queue<Goal> m_OnboardingGoals; // �º��� ��ǥ�� ��� ť
    Goal m_CurrentGoal; // ���� ��ǥ
    bool m_AllGoalsFinished; // ��� ��ǥ�� �Ϸ�Ǿ����� ����
    int m_SurfacesTapped; // �ǵ� ǥ���� ��
    int m_CurrentGoalIndex = 0; // ���� ��ǥ �ε���

    [Serializable]
    class Step
    {
        [SerializeField]
        public GameObject stepObject; // �ܰ� ��ü

        //[SerializeField]
        //public string buttonText; // ��ư �ؽ�Ʈ

        //public bool includeSkipButton; // �ǳʶٱ� ��ư ���� ����
    }

    [SerializeField]
    List<Step> m_StepList = new List<Step>(); // �ܰ� ����Ʈ

    [SerializeField]
    public TextMeshProUGUI m_StepButtonTextField; // �ܰ� ��ư �ؽ�Ʈ �ʵ�

    [SerializeField]
    public GameObject m_StartNewButton; // ����� ��ư

    [SerializeField]
    public GameObject m_ExitButton; // ���� ��ư

    [SerializeField]
    public GameObject m_ContinueButton; // ����ϱ� ��ư

    [SerializeField]
    public GameObject m_SkipButton; // �ǳʶٱ� ��ư

    [SerializeField]
    public GameObject m_QuitButton; // ��������

    [SerializeField]
    GameObject m_LearnButton; // �н� ��ư

    [SerializeField]
    GameObject m_LearnModal; // �н� ���

    [SerializeField]
    Button m_LearnModalButton; // �н� ��� ��ư

    [SerializeField]
    GameObject m_CoachingUIParent; // ��Ī UI �θ� ��ü

    [SerializeField]
    FadeMaterial m_FadeMaterial; // ���̵� ����

    [SerializeField]
    Toggle m_PassthroughToggle; // �н����� ���

    [SerializeField]
    LazyFollow m_GoalPanelLazyFollow; // ��ǥ �г� LazyFollow

    //[SerializeField]
    //GameObject m_TapTooltip; // �� ����

    //[SerializeField]
    //GameObject m_VideoPlayer; // ���� �÷��̾�

    //[SerializeField]
    //Toggle m_VideoPlayerToggle; // ���� �÷��̾� ���

    [SerializeField]
    ARPlaneManager m_ARPlaneManager; // AR ��� ������

    //[SerializeField]
    //ObjectSpawner m_ObjectSpawner; // ��ü ������

    //const int k_NumberOfSurfacesTappedToCompleteGoal = 1; // ��ǥ �ϷḦ ���� �� Ƚ��
    Vector3 m_TargetOffset = new Vector3(-.5f, -.25f, 1.5f); // ��ǥ ������

    void Start()
    {
        // �º��� ��ǥ �ʱ�ȭ
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

        //��ǥ �ϳ� ����
        m_CurrentGoal = m_OnboardingGoals.Dequeue();
        //�ʱ�ȭ
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

    // ��� ����
    void OpenModal()
    {
        if (m_LearnModal != null)
        {
            m_LearnModal.transform.localScale = Vector3.one;
        }
    }

    // ��� �ݱ�
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

        // ����� �Է� (����Ƽ ������)
#if UNITY_EDITOR
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CompleteGoal();
        }
#endif
    }

    // ��ǥ ó��
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

    // ��ǥ �Ϸ�
    void CompleteGoal()
    {
        if (m_CurrentGoal.CurrentGoal == OnboardingGoals.EnterWorld)
            //m_ObjectSpawner.objectSpawned -= OnObjectSpawned;

            // ���� ��ǥ ���� ���� ���� ��Ȱ��ȭ
            //DisableTooltips();

            m_CurrentGoal.Completed = true;
        m_CurrentGoalIndex++;
        if (m_OnboardingGoals.Count > 0)
        {
            // ť���� ���� ��ǥ�� ������
            m_CurrentGoal = m_OnboardingGoals.Dequeue();

            // ���� �ܰ��� ��ü�� ��Ȱ��ȭ
            m_StepList[m_CurrentGoalIndex - 1].stepObject.SetActive(false);

            // ���� �ܰ��� ��ü�� Ȱ��ȭ
            m_StepList[m_CurrentGoalIndex].stepObject.SetActive(true);

            // ��ư �ؽ�Ʈ�� ���� �ܰ��� ��ư �ؽ�Ʈ�� ����
            //m_StepButtonTextField.text = m_StepList[m_CurrentGoalIndex].buttonText;

            // ���� �ܰ迡 ��ŵ ��ư�� �����ϴ��� ���ο� ���� ��ŵ ��ư�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
            //m_SkipButton.SetActive(m_StepList[m_CurrentGoalIndex].includeSkipButton);
        }
        else
        {
            // ��� ��ǥ�� �Ϸ�� ���
            m_AllGoalsFinished = true;
            ForceEndAllGoals();
        }

        // ���� ��ǥ�� OnboardingGoals.FindSurfaces�� ���
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

    // ��� Ȱ��ȭ �ڷ�ƾ
    public IEnumerator TurnOnPlanes()
    {
        yield return new WaitForSeconds(1f);
        m_ARPlaneManager.enabled = true;
    }

    //// ���� ��Ȱ��ȭ
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

    // ��ǥ ���� �Ϸ�
    public void ForceCompleteGoal()
    {
        CompleteGoal();
    }

    // ��� ��ǥ ���� ����
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

    // ��Ī ����
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
    // ���α׷� ����
    public void ForceQuitApplication()
    {
        // ���ø����̼� ���� ����
        Application.Quit();

#if UNITY_EDITOR
        // Unity �����Ϳ��� ���� ���� ��� �÷��� ��带 ����
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    // ��ü ���� �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    //void OnObjectSpawned(GameObject spawnedObject)
    //{
    //    m_SurfacesTapped++;
    //    if (m_CurrentGoal.CurrentGoal == OnboardingGoals.TapSurface && m_SurfacesTapped >= k_NumberOfSurfacesTappedToCompleteGoal)
    //    {
    //        CompleteGoal();
    //        m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;
    //    }
    //}

    // ���� �÷��̾� ���
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

    // ���� �÷��̾� Ȱ��ȭ
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
