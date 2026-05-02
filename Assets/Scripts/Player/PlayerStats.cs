using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentHealthRegen;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentProjectileCount;
    [HideInInspector] public float currentProjectileSpeed;
    [HideInInspector] public float currentMagnetRadius;
    [HideInInspector] public float runtimeMaxHealth;

    [Header("Experience / Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap = 100;
    public int defaultExpCapIncrease = 50;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }
    public List<LevelRange> levelRanges;

    [Header("I-Frames")]
    public float invincibilityDuration = 0.5f;
    float invincibilityTimer;
    bool isInvincible;

    void Awake()
    {
        runtimeMaxHealth       = characterData.MaxHealth;
        currentHealth          = runtimeMaxHealth;
        currentHealthRegen     = characterData.HpRegen;
        currentMoveSpeed       = characterData.MoveSpeed;
        currentProjectileCount = characterData.ProjectileCount;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnetRadius    = characterData.MagnetRadius;
    }

    void Start()
    {
        if (levelRanges != null && levelRanges.Count > 0)
            experienceCap = levelRanges[0].experienceCapIncrease;

        SoundManager.Instance?.PlayGameMusic();
    }

    void Update()
    {
        if (invincibilityTimer > 0) invincibilityTimer -= Time.deltaTime;
        else isInvincible = false;
        Regen();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        SoundManager.Instance?.PlayGemPickup();
        while (experience >= experienceCap) LevelUp();
    }

    void LevelUp()
    {
        experience -= experienceCap;
        level++;

        int increase = 0;
        if (levelRanges != null)
            foreach (LevelRange r in levelRanges)
                if (level >= r.startLevel && level <= r.endLevel)
                    { increase = r.experienceCapIncrease; break; }

        if (increase == 0) increase = defaultExpCapIncrease;
        experienceCap += increase;

        SoundManager.Instance?.PlayLevelUp();
        FindObjectOfType<GameHUD>()?.TriggerLevelUp();
    }

    public void TakeDamage(float dmg)
    {
        if (isInvincible) return;
        currentHealth -= dmg;
        invincibilityTimer = invincibilityDuration;
        isInvincible = true;

        SoundManager.Instance?.PlayPlayerHit();
        FindObjectOfType<GameHUD>()?.PlayDamageFlash();

        if (currentHealth <= 0) Kill();
    }

    public void Kill()
    {
        FindObjectOfType<GameOverUI>()?.Show(false);
        gameObject.SetActive(false);
    }

    void Regen()
    {
        if (currentHealth < runtimeMaxHealth)
            currentHealth = Mathf.Min(currentHealth + currentHealthRegen * Time.deltaTime, runtimeMaxHealth);
    }
}
