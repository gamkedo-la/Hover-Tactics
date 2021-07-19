using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartScreen : MonoBehaviour
{
    public enum Panels { StartGame, Controllers, Credits }

    [Header("Panels")]
    [SerializeField] private GameObject Root;
    [SerializeField] private GameObject StartGamePanel;
    [SerializeField] private GameObject ControllersPanel;
    [SerializeField] private GameObject CreditsPanel;

    [Header("Buttons")]
    [SerializeField] private Button ShowControllersPanelButton;
    [SerializeField] private Button ShowCreditsPanelButton;
    [SerializeField] private Button BackButton;

    [Header("Debug")]
    [SerializeField] private bool logDebug;

    private void Awake()
    {
        Root.SetActive(true);
        ActivatePanel(Panels.StartGame);
        SetupButtonHandlers();
    }

    public void ActivatePanel(Panels panel)
    {
        if(logDebug) Debug.Log($"Activating Panel [{panel}]");
        ToggleStartGamePanel(panel == Panels.StartGame);
        ToggleControllersPanel(panel == Panels.Controllers);
        ToggleCreditsPanel(panel == Panels.Credits);

        BackButton.gameObject.SetActive(panel != Panels.StartGame);

        if(logDebug) Debug.Log($"IsBackButtonActive? : [{BackButton.gameObject.activeSelf}]");
    }

    private void SetupButtonHandlers()
    {
        BackButton.onClick.AddListener(
            () => ActivatePanel(Panels.StartGame));

        ShowControllersPanelButton.onClick.AddListener(
            () => ActivatePanel(Panels.Controllers));

        ShowCreditsPanelButton.onClick.AddListener(
            () => ActivatePanel(Panels.Credits));
    }

    private void ToggleStartGamePanel(bool isVisible)
    {
        if(logDebug) Debug.Log($"ToggleStartGamePanel [{isVisible}]");
        StartGamePanel.SetActive(isVisible);
        ShowControllersPanelButton.gameObject.SetActive(isVisible);
        ShowCreditsPanelButton.gameObject.SetActive(isVisible);
    }

    private void ToggleControllersPanel(bool isVisible)
    {
        if(logDebug) Debug.Log($"ToggleControllersPanel [{isVisible}]");
        ControllersPanel.SetActive(isVisible);
    }

    private void ToggleCreditsPanel(bool isVisible)
    {
        if(logDebug) Debug.Log($"ToggleCreditsPanel [{isVisible}]");
        CreditsPanel.SetActive(isVisible);
    }
}
