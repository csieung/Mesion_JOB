using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.UI;
using static GoalManager;

public class SettingMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_InventoryPanel; // 인벤토리 패널
    [SerializeField]
    private GameObject m_JobPanel; // 인벤토리 패널
    [SerializeField]
    private GameObject m_PetPanel; // 인벤토리 패널
    [SerializeField]
    private GameObject m_PassportPanel;
    // Start is called before the first frame update
    void Start()
    {
        m_InventoryPanel.SetActive(false); // 처음에 버튼을 비활성화할 경우
        m_JobPanel.SetActive(false);
        m_PetPanel.SetActive(false);
        m_PassportPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PushInventoryButton()
    {
        m_InventoryPanel.SetActive(!m_InventoryPanel.activeSelf);
    }

    public void PushJobButton()
    {
        m_JobPanel.SetActive(!m_JobPanel.activeSelf);
    }

    public void PushPetButton()
    {
        m_PetPanel.SetActive(!m_PetPanel.activeSelf);
    }

    public void PushPassportButton()
    {
        m_PassportPanel.SetActive(!m_PassportPanel.activeSelf);
    }

}