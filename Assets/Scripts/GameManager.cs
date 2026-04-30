using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject gameUI;
    [Header("Managers")]
    public GameSaveManager saveManager;
    [Header("Manual Settings Open")]
    public KeyCode openSettingsKey = KeyCode.O;
    [Header("Settings")]
    public GameSettings gameSettings;

    private bool wasInGameBeforeSettings = false;
    private bool gameStarted = false;

    private void Start()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(openSettingsKey))
        {
            if (settingsMenu != null && settingsMenu.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }
    }

    public void ShowMainMenu()
    {
        gameStarted = false;

        if (mainMenu != null)
            mainMenu.SetActive(true);

        if (settingsMenu != null)
            settingsMenu.SetActive(false);

        if (gameUI != null)
            gameUI.SetActive(false);

        CursorManager.SetUIMode();
    }

    public void NewGame()
    {
        if (gameSettings != null)
        {
            gameSettings.ResetSettingsToDefault();
        }
        else
        {
            Debug.LogWarning("GameSettings is not assigned on GameManager.");
        }

        if (saveManager != null)
        {
            saveManager.NewGame();
        }
        else
        {
            Debug.LogWarning("GameSaveManager is not assigned.");
        }

        StartGame();
    }

    public void ResumeGame()
    {
        if (gameSettings != null)
        {
            gameSettings.LoadSettings();
        }
        else
        {
            Debug.LogWarning("GameSettings is not assigned on GameManager.");
        }

        if (saveManager != null)
        {
            saveManager.ResumeGame();
        }
        else
        {
            Debug.LogWarning("GameSaveManager is not assigned.");
        }

        StartGame();
    }

    private void StartGame()
    {
        gameStarted = true;

        if (mainMenu != null)
            mainMenu.SetActive(false);

        if (settingsMenu != null)
            settingsMenu.SetActive(false);

        if (gameUI != null)
            gameUI.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(LockCursorAfterClick());
    }

    private IEnumerator LockCursorAfterClick()
    {
        while (Input.GetMouseButton(0))
            yield return null;

        yield return null;

        CursorManager.SetGameplayMode();
    }

    public void OpenSettings()
    {
        wasInGameBeforeSettings = gameStarted;

        if (mainMenu != null)
            mainMenu.SetActive(false);

        if (settingsMenu != null)
            settingsMenu.SetActive(true);

        if (gameUI != null)
            gameUI.SetActive(false);

        CursorManager.SetUIMode();
    }

    public void CloseSettings()
    {
        if (settingsMenu != null)
            settingsMenu.SetActive(false);

        if (wasInGameBeforeSettings)
        {
            if (mainMenu != null)
                mainMenu.SetActive(false);

            if (gameUI != null)
                gameUI.SetActive(true);

            CursorManager.SetGameplayMode();
        }
        else
        {
            if (mainMenu != null)
                mainMenu.SetActive(true);

            if (gameUI != null)
                gameUI.SetActive(false);

            CursorManager.SetUIMode();
        }
    }

    public void QuitGame()
    {
        if (saveManager != null)
            saveManager.SaveGame();

        Debug.Log("Quitting Game");
        Application.Quit();
    }
}