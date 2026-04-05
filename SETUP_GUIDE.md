# 🎮 OS Project — Инструкция по сборке UI

## Что добавлено в проект

```
Assets/Scripts/UI/
├── GameHUD.cs          ← Главный HUD (HP, XP, волна, таймер, убийства)
├── UpgradeOption.cs    ← ScriptableObject апгрейда + кнопка UI
├── GameOverUI.cs       ← Экран Game Over со статистикой
├── PauseMenuUI.cs      ← Пауза (ESC)
└── MainMenuUI.cs       ← Главное меню

Assets/Scripts/Player/PlayerStats.cs   ← обновлён (хуки на HUD + GameOver)
Assets/Scripts/Enemy/EnemyStats.cs     ← обновлён (RegisterKill)
Assets/Scripts/Enemy/EnemySpawner.cs   ← полностью переписан (работает!)
```

---

## ШАГ 1 — Сцена SampleScene: добавить HUD

### 1.1 Создать Canvas для HUD

1. Hierarchy → правая кнопка → **UI → Canvas**
2. Переименуй в `HUD Canvas`
3. Canvas Scaler → **Scale With Screen Size** → 1920×1080

### 1.2 Структура HUD (создавать UI-элементы внутри HUD Canvas)

```
HUD Canvas
├── DamageFlash        (Image, color=красный alpha=0, Stretch на весь экран)
│
├── TopBar             (Horizontal Layout Group)
│   ├── HealthSection  (Vertical Layout Group)
│   │   ├── Label      (TextMeshPro: "HP")
│   │   └── HealthSlider (Slider)
│   │       └── Fill → назначить как healthFill
│   ├── CenterSection  (Vertical Layout Group)
│   │   ├── WaveText   (TextMeshPro)
│   │   └── TimerText  (TextMeshPro)
│   └── KillSection    (Vertical Layout Group)
│       ├── Label      (TextMeshPro: "KILLS")
│       └── KillText   (TextMeshPro: "0")
│
├── ExpBar             (горизонтально внизу TopBar)
│   ├── LevelText      (TextMeshPro)
│   └── ExpSlider      (Slider)
│       └── + ExpText  (TextMeshPro поверх)
│
├── WeaponSlots        (Horizontal Layout Group, внизу экрана)
│   ├── Slot1 (Image)
│   ├── Slot2 (Image)
│   ├── Slot3 (Image)
│   └── Slot4 (Image)
│
└── LevelUpPanel       (активен=false)
    └── UpgradeContainer (Horizontal Layout Group)
        └── [сюда будут spawниться кнопки апгрейдов]
```

### 1.3 Добавить компонент GameHUD

1. Выдели `HUD Canvas`
2. Add Component → **GameHUD**
3. Заполни поля в Inspector:
   - `healthSlider` → HealthSlider
   - `healthFill` → Fill Image слайдера HP
   - `healthText` → TextMeshPro с HP текстом
   - `healthGradient` → добавь 3 ключа: красный(0), жёлтый(0.5), зелёный(1)
   - `expSlider` → ExpSlider
   - `levelText` → LevelText TMP
   - `expText` → ExpText TMP
   - `waveText` → WaveText TMP
   - `timerText` → TimerText TMP
   - `killText` → KillText TMP
   - `levelUpPanel` → LevelUpPanel объект
   - `upgradeContainer` → UpgradeContainer
   - `upgradeButtonPrefab` → (см. ШАГ 3)
   - `damageFlashImage` → DamageFlash Image
   - `possibleUpgrades` → добавь созданные апгрейды (см. ШАГ 4)

---

## ШАГ 2 — Game Over и Pause

### Game Over Panel (внутри HUD Canvas)

```
GameOverPanel (активен=false)
├── Title       (TextMeshPro — "GAME OVER")
├── LevelText   (TextMeshPro — "Level: 1")
├── KillText    (TextMeshPro — "Kills: 0")
├── TimeText    (TextMeshPro — "Time: 00:00")
├── RestartBtn  (Button)
└── MenuBtn     (Button)
```

1. Add Component → **GameOverUI** на `GameOverPanel`
2. Заполни все ссылки
3. `gameSceneName` = **SampleScene**, `mainMenuSceneName` = **MainMenu**

### Pause Panel (внутри HUD Canvas)

```
PausePanel (активен=false)
├── ResumeButton  (Button)
└── MenuButton    (Button)
```

Add Component → **PauseMenuUI**, заполни ссылки.

---

## ШАГ 3 — Prefab кнопки апгрейда

1. Создай **UI → Button** (TextMeshPro)
2. Внутри добавь:
   - `IconImage` — Image для иконки
   - `RarityBorder` — Image для цветной рамки
   - `NameText` — TMP для названия
   - `DescText` — TMP для описания
   - `RarityText` — TMP для редкости
3. Add Component → **UpgradeButtonUI**, заполни ссылки
4. Перетащи в **Assets/Prefabs** → это и будет `upgradeButtonPrefab`

---

## ШАГ 4 — Создать UpgradeOption ScriptableObjects

Assets → правая кнопка → **Create → ScriptableObjects → UpgradeOption**

Примеры:
| Название | Эффект | Rarity |
|----------|--------|--------|
| Vitality | healthBonus = 25 | Common |
| Swift Feet | moveSpeedBonus = 1.5 | Rare |
| Twin Shot | projectileBonus = 1 | Rare |
| Vampire | healthRegenBonus = 2 | Epic |
| Mega Magnet | magnetRadiusBonus = 2 | Legendary |

Добавь их в поле `possibleUpgrades` компонента `GameHUD`.

---

## ШАГ 5 — Сцена MainMenu (опционально)

1. File → New Scene → сохрани как **MainMenu**
2. Создай Canvas
3. Внутри создай панели: MainPanel, SettingsPanel, CreditsPanel
4. В MainPanel: кнопки Play, Settings, Credits, Quit
5. В SettingsPanel: 3 Slider (громкость), кнопка Back
6. В CreditsPanel: текст + кнопка Back
7. Add Component → **MainMenuUI** на Canvas root
8. Заполни все ссылки, `gameSceneName` = **SampleScene**
9. **File → Build Settings**: добавь MainMenu (index 0), SampleScene (index 1)

---

## ШАГ 6 — EnemySpawner настройка

1. Найди объект EnemySpawner в сцене
2. В Inspector → **Waves**: добавь волны
3. Каждая волна → Enemy Groups → укажи prefab врага и количество
4. `Spawn Interval` = 0.5, `Spawn Radius` = 12, `Wave Start Delay` = 2

---

## Зависимости (должны быть в Package Manager)

- ✅ **TextMeshPro** (Window → Package Manager → установить)
- ✅ **Unity UI**
- ✅ **Input System** (опционально)

---

## Порядок сцен в Build Settings

| Index | Сцена |
|-------|-------|
| 0 | MainMenu |
| 1 | SampleScene |
