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
    [SerializeField] private bool DarkLayer = false;

    [Header("Touch")]
    [SerializeField] private GameObject touchControls;

    private void OnEnable()
    {
        if(StartGameScreen) StartGameScreen.OnMainButtonPressed += HandleStartScreenMainButtonPressed;
        if(PauseGameScreen) PauseGameScreen.OnMainButtonPressed += HandlePauseScreenMainButtonPressed; 

        // menu button clicks
        if(StartGameScreen) StartGameScreen.OnButtonPressed += PlaySelectedSound;
        if(PauseGameScreen) PauseGameScreen.OnButtonPressed += PlaySelectedSound;
    }

    private void OnDisable()
    {
        if(StartGameScreen) StartGameScreen.OnMainButtonPressed -= HandleStartScreenMainButtonPressed;
        if(PauseGameScreen) PauseGameScreen.OnMainButtonPressed += HandlePauseScreenMainButtonPressed;

        // menu button clicks
        if(StartGameScreen) StartGameScreen.OnButtonPressed -= PlaySelectedSound;
        if(PauseGameScreen) PauseGameScreen.OnButtonPressed -= PlaySelectedSound;
    }

    private bool IsAnyActive()
    {
        for(int i = (DarkLayer ? 1 : 0); i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if(DarkLayer) transform.GetChild(0).gameObject.SetActive(IsAnyActive());
    }

    public void ToggleStartGameScreen(bool isVisible, bool fadeFx = true)
    {
        if(fadeFx == false)
        {
            if(StartGameScreen) StartGameScreen.gameObject.SetActive(isVisible);
            return;
        }

        StartGameScreenFadeFx.Fade(isVisible, () => {
            StartGameScreen.gameObject.SetActive(isVisible);
        });
    }

    public bool TogglePauseScreen(bool isVisible, bool fadeFx = true)
    {
        if(isVisible && IsAnyActive()) return false;

        if(fadeFx == false)
        {
            if(PauseGameScreen)
            {
                PauseGameScreen.gameObject.SetActive(isVisible);
                if(GameManager.instance && GameManager.instance.touch) touchControls.SetActive(!isVisible);
            }
            return true;
        }

        PauseGameScreenFadeFX.Fade(isVisible, () => {
            PauseGameScreen.gameObject.SetActive(isVisible);
        });
        return true;
    }

    private void HandleStartScreenMainButtonPressed()
    {
        OnStartGameButtonPressed?.Invoke();
    }

    private void HandlePauseScreenMainButtonPressed()
    {
        OnResumeGameButtonPressed?.Invoke();
    }

    private void PlaySelectedSound()
    {
        SoundFXManager.PlayOneShot(SoundFxKey.SELECT);
    }
}
