using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameMenu gameMenu;

    [Header("Starting States (and current states)")]
    public bool isShowingStartGameMenu;
    public bool isShowingPauseGameMenu;

    static public bool playNight = false;

    [Header("Debug")]
    [SerializeField] private bool logDebug;

    private void Start()
    {
        gameMenu.ToggleStartGameScreen(isShowingStartGameMenu, fadeFx: false);
        gameMenu.TogglePauseScreen(isShowingPauseGameMenu, fadeFx: false);
    }

    private void OnEnable()
    {
        gameMenu.OnStartGameButtonPressed += GameStarted;
        gameMenu.OnResumeGameButtonPressed += GameResumed; 
    }

    private void OnDisable()
    {
        gameMenu.OnStartGameButtonPressed -= GameStarted;
        gameMenu.OnResumeGameButtonPressed += GameResumed;
    }

    private void GameStarted()
    {
        // hide panel
        //isShowingStartGameMenu = false;
        //gameMenu.ToggleStartGameScreen(isShowingStartGameMenu, false);

        // do something whenever the start game button has been pressed
        Time.timeScale = 1.0f;
        //if(playNight) SceneManager.LoadScene("PlayNight");
        //else SceneManager.LoadScene("Play");
        if(playNight) FadeToScene.Load("PlayNight");
        else FadeToScene.Load("Play");
    }

    private void GameResumed()
    {
        // hide panel
        isShowingPauseGameMenu = false;
        gameMenu.TogglePauseScreen(isShowingPauseGameMenu, false);

        // do something whenever the resume game button has been pressed
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel")
        && SceneManager.GetActiveScene().name.Contains("Play"))
        {
            isShowingPauseGameMenu = !isShowingPauseGameMenu;
            gameMenu.TogglePauseScreen(isShowingPauseGameMenu, false);
            if(isShowingPauseGameMenu) Time.timeScale = 0.0f;
            else Time.timeScale = 1.0f;
        }
    }
}
