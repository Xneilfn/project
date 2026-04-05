using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause menu. Attach to a hidden panel. ESC toggles pause.
/// </summary>
public class PauseMenuUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button mainMenuButton;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    bool _paused;

    void Start()
    {
        if (panel) panel.SetActive(false);
        resumeButton?.onClick.AddListener(Resume);
        mainMenuButton?.onClick.AddListener(GoMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    void Toggle()
    {
        _paused = !_paused;
        if (panel) panel.SetActive(_paused);
        Time.timeScale = _paused ? 0f : 1f;
    }

    void Resume()
    {
        _paused = false;
        if (panel) panel.SetActive(false);
        Time.timeScale = 1f;
    }

    void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
