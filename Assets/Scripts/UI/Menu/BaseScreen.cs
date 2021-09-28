using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class BaseScreen : MonoBehaviour
{
    public enum Panels { None, Main, Controllers, Credits }
    public Action<Panels> OnPanelActivated;
    public Action OnButtonPressed;
    public Action OnMainButtonPressed;

    [Header("Panels")]
    [SerializeField] private GameObject Root;
    [SerializeField] private GameObject MainScreenPanel;
    [SerializeField] private GameObject ControllersPanel;
    [SerializeField] private GameObject CreditsPanel;

    [Header("Buttons")]
    [SerializeField] private Button MainButtonPressed;
    [SerializeField] private Button ShowControllersPanelButton;
    [SerializeField] private Button ShowCreditsPanelButton;
    [SerializeField] private Button BackButton;

    [Header("Debug")]
    [SerializeField] private bool logDebug;

    private void Awake()
    {
        Root.SetActive(true);
        ActivatePanel(Panels.Main);
        SetupButtonHandlers();
    }

    public void ActivatePanel(Panels panel)
    {
        if(logDebug) Debug.Log($"Activating Panel [{panel}]");

        OnPanelActivated?.Invoke(panel);
        
        ToggleMainPanel(panel == Panels.Main);
        ToggleControllersPanel(panel == Panels.Controllers);
        ToggleCreditsPanel(panel == Panels.Credits);

        BackButton.gameObject.SetActive(panel != Panels.Main);

        if(logDebug) Debug.Log($"IsBackButtonActive? : [{BackButton.gameObject.activeSelf}]");
    }

    private void SetupButtonHandlers()
    {
        MainButtonPressed.onClick.AddListener(
            () => OnMainButtonPressed?.Invoke());

        BackButton.onClick.AddListener(
            () => MenuButtonPressed(Panels.Main));

        ShowControllersPanelButton.onClick.AddListener(
            () => MenuButtonPressed(Panels.Controllers));

        ShowCreditsPanelButton.onClick.AddListener(
            () => MenuButtonPressed(Panels.Credits));
    }

    private void MenuButtonPressed(Panels panel)
    {
        OnButtonPressed?.Invoke();
        ActivatePanel(panel);
    }

    private void ToggleMainPanel(bool isVisible)
    {
        if(logDebug) Debug.Log($"ToggleMainPanel [{isVisible}]");
        MainScreenPanel.SetActive(isVisible);
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

    public void QuitToMenu()
    {
        Time.timeScale = 1.0f;
        //SceneManager.LoadScene("Start");
        FadeToScene.Load("Start");
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FadeToScene.Load(SceneManager.GetActiveScene().name);
    }

    public void ToggleBGM(TextMeshProUGUI textObject)
    {
        MusicManager.state = !MusicManager.state;
        textObject.GetComponent<TextMeshProUGUI>().text = MusicManager.state ? "Music: ON" : "Music: OFF";
    }

    public void ToggleSFX(TextMeshProUGUI textObject)
    {
        SoundFXManager.state = !SoundFXManager.state;
        textObject.GetComponent<TextMeshProUGUI>().text = SoundFXManager.state ? "Sound Effects: ON" : "Sound Effects: OFF";
    }

    public void ToggleDark(TextMeshProUGUI textObject)
    {
        GameMenuController.playNight = !GameMenuController.playNight;
        textObject.GetComponent<TextMeshProUGUI>().text = GameMenuController.playNight ? "Dark Mode: ON" : "Dark Mode: OFF";
    }
}
