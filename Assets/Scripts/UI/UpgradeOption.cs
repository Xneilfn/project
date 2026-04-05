using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ─── Rarity ──────────────────────────────────────────────────────────────────
public enum UpgradeRarity { Common, Rare, Epic, Legendary }

// ─── UpgradeOption ScriptableObject ─────────────────────────────────────────
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
        player.characterData.MaxHealth += healthBonus;
        player.currentHealth           = Mathf.Min(player.currentHealth + healthBonus, player.characterData.MaxHealth);
        player.currentHealthRegen      += healthRegenBonus;
        player.currentMoveSpeed        += moveSpeedBonus;
        player.currentProjectileCount  += projectileBonus;
        player.currentProjectileSpeed  += projectileSpeedBonus;
        player.currentMagnetRadius     += magnetRadiusBonus;
    }
}