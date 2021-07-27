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
    [SerializeField] private FadeUIFX StartGameScreenFadeFx;
    [SerializeField] private BaseScreen PauseGameScreen;
    [SerializeField] private FadeUIFX PauseGameScreenFadeFX;

    [Header("Debug")]
    [SerializeField] private bool logDebug;

    private void OnEnable()
    {
        StartGameScreen.OnMainButtonPressed += HandleStartScreenMainButtonPressed;
        PauseGameScreen.OnMainButtonPressed += HandlePauseScreenMainButtonPressed; 

        // menu button clicks
        StartGameScreen.OnButtonPressed += PlaySelectedSound;
        PauseGameScreen.OnButtonPressed += PlaySelectedSound;
    }

    private void OnDisable()
    {
        StartGameScreen.OnMainButtonPressed -= HandleStartScreenMainButtonPressed;
        PauseGameScreen.OnMainButtonPressed += HandlePauseScreenMainButtonPressed;

        // menu button clicks
        StartGameScreen.OnButtonPressed -= PlaySelectedSound;
        PauseGameScreen.OnButtonPressed -= PlaySelectedSound;
    }

    public void ToggleStartGameScreen(bool isVisible, bool fadeFx = true)
    {
        if(fadeFx == false)
        {
            StartGameScreen.gameObject.SetActive(isVisible);
            return;
        }

        StartGameScreenFadeFx.Fade(isVisible, () => {
            StartGameScreen.gameObject.SetActive(isVisible);
        });
    }

    public void TogglePauseScreen(bool isVisible, bool fadeFx = true)
    {
        if(fadeFx == false)
        {
            PauseGameScreen.gameObject.SetActive(isVisible);
            return;
        }

        PauseGameScreenFadeFX.Fade(isVisible, () => {
            PauseGameScreen.gameObject.SetActive(isVisible);
        });
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

    private void PlaySelectedSound()
    {
        SoundFXManager.PlayOneShot(SoundFxKey.Select);
    }
}
