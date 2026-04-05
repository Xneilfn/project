using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Main Menu controller. Attach to the Canvas root in the MainMenu scene.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Main Panel Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("Settings")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Button settingsBackButton;

    [Header("Credits")]
    public Button creditsBackButton;

    [Header("Scene")]
    public string gameSceneName = "SampleScene";

    void Start()
    {
        playButton?.onClick.AddListener(Play);
        settingsButton?.onClick.AddListener(() => SwitchTo(settingsPanel));
        creditsButton?.onClick.AddListener(() => SwitchTo(creditsPanel));
        quitButton?.onClick.AddListener(Quit);
        settingsBackButton?.onClick.AddListener(() => SwitchTo(mainPanel));
        creditsBackButton?.onClick.AddListener(() => SwitchTo(mainPanel));

        // Restore volume settings
        float mv = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float mu = PlayerPrefs.GetFloat("MusicVolume",  0.8f);
        float sx = PlayerPrefs.GetFloat("SFXVolume",    1f);

        if (masterVolumeSlider) { masterVolumeSlider.value = mv; masterVolumeSlider.onValueChanged.AddListener(v => { AudioListener.volume = v; PlayerPrefs.SetFloat("MasterVolume", v); }); }
        if (musicVolumeSlider)  { musicVolumeSlider.value  = mu; musicVolumeSlider.onValueChanged.AddListener(v  => PlayerPrefs.SetFloat("MusicVolume",  v)); }
        if (sfxVolumeSlider)    { sfxVolumeSlider.value    = sx; sfxVolumeSlider.onValueChanged.AddListener(v    => PlayerPrefs.SetFloat("SFXVolume",    v)); }

        AudioListener.volume = mv;
        SwitchTo(mainPanel);
    }

    void SwitchTo(GameObject target)
    {
        if (mainPanel)     mainPanel.SetActive(target == mainPanel);
        if (settingsPanel) settingsPanel.SetActive(target == settingsPanel);
        if (creditsPanel)  creditsPanel.SetActive(target == creditsPanel);
    }

    void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
