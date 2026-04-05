using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Health")]
    public Slider healthSlider;
    public Image  healthFill;
    public TextMeshProUGUI healthText;
    public Gradient healthGradient;

    [Header("Experience")]
    public Slider expSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;

    [Header("Wave & Timer")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;

    [Header("Kill Counter")]
    public TextMeshProUGUI killText;

    [Header("Level-Up Panel")]
    public GameObject      levelUpPanel;
    public Transform       upgradeContainer;
    public GameObject      upgradeButtonPrefab;

    [Header("Damage Flash")]
    public Image damageFlashImage;

    [Header("Weapon Slots")]
    public List<Image> weaponSlotIcons;

    [Header("Upgrades")]
    public List<UpgradeOption> possibleUpgrades;

    PlayerStats _player;
    int   _kills;
    float _timer;
    bool  _paused;

    void Start()
    {
        _player = FindObjectOfType<PlayerStats>();
        if (levelUpPanel) levelUpPanel.SetActive(false);
        if (damageFlashImage) damageFlashImage.color = new Color(1, 0, 0, 0);
        SetWave(1);
    }

    void Update()
    {
        if (_player == null || _paused) return;

        float maxHp = _player.characterData.MaxHealth;
        float ratio = Mathf.Clamp01(_player.currentHealth / maxHp);
        if (healthSlider) healthSlider.value = ratio;
        if (healthFill)   healthFill.color   = healthGradient.Evaluate(ratio);
        if (healthText)   healthText.text    = $"{Mathf.CeilToInt(_player.currentHealth)}/{Mathf.CeilToInt(maxHp)}";

        float expRatio = _player.experienceCap > 0 ? (float)_player.experience / _player.experienceCap : 0f;
        if (expSlider) expSlider.value = expRatio;
        if (levelText) levelText.text  = $"Lv {_player.level}";
        if (expText)   expText.text    = $"{_player.experience}/{_player.experienceCap}";

        _timer += Time.deltaTime;
        int m = Mathf.FloorToInt(_timer / 60f);
        int s = Mathf.FloorToInt(_timer % 60f);
        if (timerText) timerText.text = $"{m:00}:{s:00}";
    }

    public void SetWave(int wave)
    {
        if (waveText) waveText.text = $"Wave {wave}";
    }

    public void RegisterKill()
    {
        _kills++;
        if (killText) killText.text = _kills.ToString();
    }

    public void TriggerLevelUp()
    {
        // Если нет апгрейдов или prefab — просто не останавливаем игру
        if (possibleUpgrades == null || possibleUpgrades.Count == 0)
        {
            Debug.Log("[GameHUD] Level up! No upgrades configured — continuing.");
            return;
        }
        if (upgradeButtonPrefab == null || upgradeContainer == null)
        {
            Debug.LogWarning("[GameHUD] Level up! upgradeButtonPrefab or upgradeContainer not assigned — continuing.");
            return;
        }

        List<UpgradeOption> pool = new List<UpgradeOption>(possibleUpgrades);
        List<UpgradeOption> chosen = new List<UpgradeOption>();
        int count = Mathf.Min(3, pool.Count);
        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, pool.Count);
            chosen.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        ShowLevelUpPanel(chosen);
    }

    void ShowLevelUpPanel(List<UpgradeOption> options)
    {
        Time.timeScale = 0f;
        _paused = true;
        if (levelUpPanel) levelUpPanel.SetActive(true);

        foreach (Transform child in upgradeContainer)
            Destroy(child.gameObject);

        foreach (var opt in options)
        {
            var btn = Instantiate(upgradeButtonPrefab, upgradeContainer);
            var ui  = btn.GetComponent<UpgradeButtonUI>();
            ui?.Init(opt, OnUpgradeChosen);
        }
    }

    void OnUpgradeChosen(UpgradeOption opt)
    {
        opt.Apply(_player);
        if (levelUpPanel) levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
        _paused = false;
    }

    public void PlayDamageFlash()
    {
        if (damageFlashImage) StartCoroutine(DamageFlashRoutine());
    }

    IEnumerator DamageFlashRoutine()
    {
        damageFlashImage.color = new Color(1, 0, 0, 0.35f);
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.unscaledDeltaTime;
            damageFlashImage.color = new Color(1, 0, 0, Mathf.Lerp(0.35f, 0f, t / 0.3f));
            yield return null;
        }
        damageFlashImage.color = new Color(1, 0, 0, 0);
    }

    public float GetTimeSurvived() => _timer;
    public int   GetKills()        => _kills;
}