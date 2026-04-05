using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public GameObject enemyPrefab;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public float spawnInterval = 0.8f;
    }

    [Header("Волны")]
    public List<Wave> waves;

    [Header("Настройки спавна")]
    public float spawnRadius = 12f;
    public float waveStartDelay = 3f;
    // Множитель увеличения количества врагов с каждой новой волной
    public float difficultyMultiplier = 1.3f;
    // Интервал между циклами волн (когда все волны пройдены — начинаем сначала сложнее)
    public float betweenWavesDelay = 5f;

    Transform _player;
    int  _currentWave  = 0;
    int  _cycleCount   = 0;   // сколько раз прошли все волны

    void Start()
    {
        _player = FindObjectOfType<Player>()?.transform;
        if (_player == null) { Debug.LogError("[EnemySpawner] Player not found!"); return; }
        if (waves == null || waves.Count == 0) { Debug.LogError("[EnemySpawner] No waves configured!"); return; }

        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (true)   // бесконечный цикл волн
        {
            // Сообщаем HUD
            int displayWave = _cycleCount * waves.Count + _currentWave + 1;
            FindObjectOfType<GameHUD>()?.SetWave(displayWave);

            yield return new WaitForSeconds(waveStartDelay);

            yield return StartCoroutine(SpawnWave(waves[_currentWave]));

            yield return new WaitForSeconds(betweenWavesDelay);

            _currentWave++;
            if (_currentWave >= waves.Count)
            {
                _currentWave = 0;
                _cycleCount++;
                // с каждым циклом волны становятся тяжелее
                difficultyMultiplier *= 1.2f;
            }
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        // Считаем общее число врагов с учётом сложности
        int totalToSpawn = 0;
        foreach (var g in wave.enemyGroups)
            totalToSpawn += Mathf.RoundToInt(g.enemyCount * Mathf.Pow(difficultyMultiplier, _cycleCount));

        int spawned = 0;
        int groupIndex = 0;

        while (spawned < totalToSpawn)
        {
            if (_player == null) yield break;

            EnemyGroup group = wave.enemyGroups[groupIndex % wave.enemyGroups.Count];
            groupIndex++;

            // Пропускаем группу если prefab не назначен
            if (group.enemyPrefab == null)
            {
                Debug.LogError($"[EnemySpawner] enemyPrefab не назначен в группе '{group.enemyName}' волны '{wave.waveName}'! Назначь Bat prefab в Inspector.");
                spawned++;
                continue;
            }

            // Спавним за экраном вокруг игрока
            Vector2 spawnPos = (Vector2)_player.position + Random.insideUnitCircle.normalized * spawnRadius;
            Instantiate(group.enemyPrefab, spawnPos, Quaternion.identity);
            spawned++;

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}
