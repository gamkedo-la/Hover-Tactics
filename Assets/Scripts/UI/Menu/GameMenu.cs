using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMenu : MonoBehaviour
{
    public Action OnStartGameButtonPressed;
    public Action OnResumeGameButtonPressed;

    [Header("Game Screens")]
    [SerializeField] private BaseScreen StartGameScreen;
    [SerializeField] private BaseScreen PauseGameScreen;

    [Header("Debug")]
    [SerializeField] private bool logDebug;

    private void OnEnable()
    {
        StartGameScreen.OnMainButtonPressed += HandleStartScreenMainButtonPressed;
        PauseGameScreen.OnMainButtonPressed += HandlePauseScreenMainButtonPressed; 
    }

    private void OnDisable()
    {
        StartGameScreen.OnMainButtonPressed -= HandleStartScreenMainButtonPressed;
        PauseGameScreen.OnMainButtonPressed += HandlePauseScreenMainButtonPressed;
    }

    public void ToggleStartGameScreen(bool isVisible)
    {
        StartGameScreen.gameObject.SetActive(isVisible);
    }
    
    public void TogglePauseScreen(bool isVisible)
    {
        PauseGameScreen.gameObject.SetActive(isVisible);
    }

    private void HandleStartScreenMainButtonPressed()
    {
        if (logDebug) Debug.Log("Start Game Button Pressed");
        OnStartGameButtonPressed?.Invoke();
    }

    private void HandlePauseScreenMainButtonPressed()
    {
        if (logDebug) Debug.Log("Resume Game Button Pressed");
        OnResumeGameButtonPressed?.Invoke();
    }
}
