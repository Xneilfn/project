using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Game Over screen. Attach to a hidden panel in the game scene.
/// Call Show(won) from PlayerStats.Kill().
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Stats Labels")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI waveText;

    [Header("Buttons")]
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Scene Names")]
    public string gameSceneName     = "SampleScene";
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        if (panel) panel.SetActive(false);
        restartButton?.onClick.AddListener(Restart);
        mainMenuButton?.onClick.AddListener(GoMainMenu);
    }

    public void Show(bool won)
    {
        Time.timeScale = 0f;
        if (panel) panel.SetActive(true);

        if (titleText) titleText.text = won ? "YOU WIN!" : "GAME OVER";

        GameHUD hud = FindObjectOfType<GameHUD>(true);
        PlayerStats ps = FindObjectOfType<PlayerStats>(true);

        if (ps != null)
        {
            if (levelText) levelText.text = $"Level Reached: {ps.level}";
        }
        if (hud != null)
        {
            if (killText) killText.text = $"Enemies Killed: {hud.GetKills()}";
            float t = hud.GetTimeSurvived();
            int m = Mathf.FloorToInt(t / 60f);
            int s = Mathf.FloorToInt(t % 60f);
            if (timeText) timeText.text = $"Time: {m:00}:{s:00}";
        }
    }

    void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
