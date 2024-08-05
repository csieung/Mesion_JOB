using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager_tg : MonoBehaviour
{
    public SaveManager saveManager;
    public LoadManager loadManager;
    public TextMeshProUGUI statusText; // Text Mesh Pro UI ≈ÿΩ∫∆Æ

    [SerializeField]
    private InputActionReference saveAction;
    [SerializeField]
    private InputActionReference loadAction;

    private void OnEnable()
    {
        saveAction.action.Enable();
        loadAction.action.Enable();
    }

    private void OnDisable()
    {
        saveAction.action.Disable();
        loadAction.action.Disable();
    }

    void Update()
    {
        if (saveAction.action.triggered)
        {
            saveManager.Save();
            ShowStatusMessage("Game Saved!");
        }

        if (loadAction.action.triggered)
        {
            loadManager.Load();
            ShowStatusMessage("Game Loaded!");
        }
    }

    void ShowStatusMessage(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
            StopCoroutine(ClearStatusMessage());
            StartCoroutine(ClearStatusMessage());
        }
    }

    IEnumerator ClearStatusMessage()
    {
        yield return new WaitForSeconds(2);
        statusText.text = "";
    }
}
