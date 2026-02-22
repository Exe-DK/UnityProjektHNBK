using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Füge den Namespace für EventSystem hinzu

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject gameUI;
    public GameObject pauseMenuUI;
    public GameObject aktionfelder;
    public GameObject setupMenuUI;//NEU

    void Start()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogError("PauseMenuUI nicht zugewiesen!");
            return;
        }

        pauseMenuUI.SetActive(false);
        setupMenuUI.SetActive(false);//NEU
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameIsPaused || Time.timeScale == 0) //NEU zusätzl. Bedingung wegen Infofenstern
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // Überprüfen, ob der Cursor über einem UI-Element ist und sichtbar ist
        if (Cursor.visible && IsCursorOverUI())
        {
            // Stelle sicher, dass der Cursor sichtbar ist
            Cursor.visible = true;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;

        if (gameUI != null)
        {
            gameUI.SetActive(false);
        }
        if (aktionfelder != null)
        {
            aktionfelder.SetActive(false);
        }

        Cursor.visible = true; //NEU 
        pauseMenuUI.SetActive(true);

        var inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
        {
            inputManager.enabled = false;
        }
    }

    [System.Obsolete]
    public void ResumeGame()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (gameUI != null)
        {
            gameUI.SetActive(true);
        }
        if (aktionfelder != null)
        {
            aktionfelder.SetActive(true);
        }

        Cursor.visible = false;//NEU
        pauseMenuUI.SetActive(false);
        setupMenuUI.SetActive(false);//NEU

        var inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
        {
            inputManager.enabled = true;
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    //NEU
    public void SetupMenu()
    {
        pauseMenuUI.SetActive(false);
        setupMenuUI.SetActive(true);
    }

    public void Back()
    {
        pauseMenuUI.SetActive(true);
        setupMenuUI.SetActive(false);
    }
    //NEU ENDE
    public void QuitGame()
    {
        Application.Quit();
    }

    // Überprüfen, ob der Cursor über einem UI-Element ist
    bool IsCursorOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
