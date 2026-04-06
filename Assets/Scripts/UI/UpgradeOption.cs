using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UpgradeRarity { Common, Rare, Epic, Legendary }

[CreateAssetMenu(fileName = "UpgradeOption", menuName = "ScriptableObjects/UpgradeOption")]
public class UpgradeOption : ScriptableObject
{
    [Header("Display")]
    public string upgradeName = "Upgrade";
    [TextArea] public string description = "";
    public Sprite icon;
    public UpgradeRarity rarity = UpgradeRarity.Common;

    [Header("Stat Bonuses")]
    public float healthBonus          = 0f;
    public float healthRegenBonus     = 0f;
    public float moveSpeedBonus       = 0f;
    public int   projectileBonus      = 0;
    public float projectileSpeedBonus = 0f;
    public float magnetRadiusBonus    = 0f;

    public void Apply(PlayerStats player)
    {
        // HP бонус — добавляем только к текущему здоровью и runtime-максимуму
        // НЕ трогаем characterData.MaxHealth (ScriptableObject) чтобы не сохранять между сессиями
        if (healthBonus > 0)
        {
            // Увеличиваем только runtime-значение через временный максимум
            float newMax = player.characterData.MaxHealth + healthBonus;
            player.currentHealth = Mathf.Min(player.currentHealth + healthBonus, newMax);
            // Сохраняем увеличенный макс только в рантайм-поле
            player.runtimeMaxHealth += healthBonus;
        }

        player.currentHealthRegen     += healthRegenBonus;
        player.currentMoveSpeed       += moveSpeedBonus;
        player.currentProjectileCount += projectileBonus;
        player.currentProjectileSpeed += projectileSpeedBonus;
        player.currentMagnetRadius    += magnetRadiusBonus;

        // Обновляем радиус коллектора если изменился magnet
        if (magnetRadiusBonus != 0)
        {
            PlayerCollector col = player.GetComponentInChildren<PlayerCollector>();
            if (col != null)
            {
                CircleCollider2D circle = col.GetComponent<CircleCollider2D>();
                if (circle != null) circle.radius = player.currentMagnetRadius;
            }
        }
    }
}