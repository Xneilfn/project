using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// ─── UpgradeButtonUI ─────────────────────────────────────────────────────────
public class UpgradeButtonUI : MonoBehaviour
{
    [Header("References")]
    public Image            iconImage;
    public Image            rarityBorder;
    public TextMeshProUGUI  nameText;
    public TextMeshProUGUI  descText;
    public TextMeshProUGUI  rarityText;
    public Button           button;

    [Header("Rarity Colors")]
    public Color commonColor    = new Color(0.75f, 0.75f, 0.75f);
    public Color rareColor      = new Color(0.20f, 0.55f, 1.00f);
    public Color epicColor      = new Color(0.65f, 0.10f, 1.00f);
    public Color legendaryColor = new Color(1.00f, 0.72f, 0.00f);

    Action<UpgradeOption> _callback;
    UpgradeOption _option;

    public void Init(UpgradeOption option, Action<UpgradeOption> callback)
    {
        _option   = option;
        _callback = callback;

        if (nameText)   nameText.text   = option.upgradeName;
        if (descText)   descText.text   = option.description;
        if (rarityText) rarityText.text = option.rarity.ToString().ToUpper();
        if (iconImage && option.icon) iconImage.sprite = option.icon;

        Color c = GetRarityColor(option.rarity);
        if (rarityBorder)     rarityBorder.color     = c;
        if (rarityText)       rarityText.color        = c;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => _callback?.Invoke(_option));
    }

    Color GetRarityColor(UpgradeRarity r) => r switch
    {
        UpgradeRarity.Rare      => rareColor,
        UpgradeRarity.Epic      => epicColor,
        UpgradeRarity.Legendary => legendaryColor,
        _                       => commonColor
    };
}
