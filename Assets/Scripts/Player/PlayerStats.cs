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

    // Сколько опыта нужно добавлять к капу на каждый уровень
    // (используется если levelRanges пустой или уровень вышел за границы)
    [Tooltip("Прирост capа за каждый уровень если нет подходящего LevelRange")]
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
        currentHealth          = characterData.MaxHealth;
        currentHealthRegen     = characterData.HpRegen;
        currentMoveSpeed       = characterData.MoveSpeed;
        currentProjectileCount = characterData.ProjectileCount;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnetRadius    = characterData.MagnetRadius;
    }

    void Start()
    {
        // Устанавливаем начальный cap из первого LevelRange если он есть
        if (levelRanges != null && levelRanges.Count > 0)
            experienceCap = levelRanges[0].experienceCapIncrease;
        // Иначе остаётся значение из Inspector (по умолчанию 100)
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
        // Цикл — на случай если за раз набрали опыт на несколько уровней
        while (experience >= experienceCap)
            LevelUp();
    }

    void LevelUp()
    {
        experience -= experienceCap;
        level++;

        // Ищем подходящий LevelRange для нового уровня
        int increase = 0;
        if (levelRanges != null)
        {
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    increase = range.experienceCapIncrease;
                    break;
                }
            }
        }

        // Если диапазон не найден — используем дефолтный прирост
        if (increase == 0)
            increase = defaultExpCapIncrease;

        experienceCap += increase;

        Debug.Log($"[PlayerStats] Level Up! Уровень: {level}, новый cap: {experienceCap}");

        // Показываем панель апгрейдов
        GameHUD hud = FindObjectOfType<GameHUD>();
        hud?.TriggerLevelUp();
    }

    public void TakeDamage(float dmg)
    {
        if (isInvincible) return;
        currentHealth -= dmg;
        invincibilityTimer = invincibilityDuration;
        isInvincible = true;

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
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentHealthRegen * Time.deltaTime;
            currentHealth  = Mathf.Min(currentHealth, characterData.MaxHealth);
        }
    }
}
