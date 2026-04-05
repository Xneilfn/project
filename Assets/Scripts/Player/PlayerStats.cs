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

    [Header("Experience / Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap = 100;

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
        currentHealth        = characterData.MaxHealth;
        currentHealthRegen   = characterData.HpRegen;
        currentMoveSpeed     = characterData.MoveSpeed;
        currentProjectileCount = characterData.ProjectileCount;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnetRadius  = characterData.MagnetRadius;
    }

    void Start()
    {
        if (levelRanges != null && levelRanges.Count > 0)
            experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        if (invincibilityTimer > 0)
            invincibilityTimer -= Time.deltaTime;
        else
            isInvincible = false;

        Regen();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int increase = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    increase = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += increase;

            // Show upgrade panel
            GameHUD hud = FindObjectOfType<GameHUD>();
            if (hud != null) hud.TriggerLevelUp();
        }
    }

    public void TakeDamage(float dmg)
    {
        if (isInvincible) return;
        currentHealth -= dmg;
        invincibilityTimer = invincibilityDuration;
        isInvincible = true;

        GameHUD hud = FindObjectOfType<GameHUD>();
        hud?.PlayDamageFlash();

        if (currentHealth <= 0) Kill();
    }

    public void Kill()
    {
        GameOverUI go = FindObjectOfType<GameOverUI>();
        go?.Show(false);
        gameObject.SetActive(false);
    }

    void Regen()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentHealthRegen * Time.deltaTime;
            currentHealth  = Mathf.Min(currentHealth, characterData.MaxHealth);
        }
    }
}
