using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private GameMenu gameMenu;

    public bool isShowingStartGameMenu;
    public bool isShowingPauseGameMenu;

    [Header("Debug")]
    [SerializeField] private bool logDebug;

    private void Start()
    {
        gameMenu.ToggleStartGameScreen(isShowingStartGameMenu);
        gameMenu.TogglePauseScreen(isShowingPauseGameMenu);
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
        if(logDebug) Debug.Log("START GAME");
        
        // hide panel
        isShowingStartGameMenu = false;
        gameMenu.ToggleStartGameScreen(isShowingStartGameMenu);

        // do something whenever the start game button has been pressed
    }

    private void GameResumed()
    {
        if(logDebug) Debug.Log("RESUME GAME");

        // hide panel
        isShowingPauseGameMenu = false;
        gameMenu.TogglePauseScreen(isShowingPauseGameMenu);

        // do something whenever the resume game button has been pressed
    }

    private void Update()
    {
        // EXAMPLE ON HOW TO CONTROL THE GAME MENU
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isShowingStartGameMenu = !isShowingStartGameMenu;
            gameMenu.ToggleStartGameScreen(isShowingStartGameMenu);
        }

        if(Input.GetKeyDown(KeyCode.F10))
        {
            isShowingPauseGameMenu = !isShowingPauseGameMenu;
            gameMenu.TogglePauseScreen(isShowingPauseGameMenu);
        }
    }
}
