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
        [HideInInspector] public int spawnCount;
        public GameObject enemyPrefab;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        public float spawnInterval;
        [HideInInspector] public int spawnCount;
    }

    [Header("Waves")]
    public List<Wave> waves;
    public int currentWaveIndex = 0;

    [Header("Spawn Settings")]
    public float spawnRadius = 10f;   // distance from player to spawn
    public float waveStartDelay = 2f;

    Transform playerTransform;
    bool waveActive = false;

    void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        StartCoroutine(StartWave(currentWaveIndex));
    }

    IEnumerator StartWave(int index)
    {
        if (index >= waves.Count) yield break;

        Wave wave = waves[index];
        CalculateWaveQuota(wave);

        GameHUD hud = FindObjectOfType<GameHUD>();
        hud?.SetWave(index + 1);

        yield return new WaitForSeconds(waveStartDelay);

        waveActive = true;
        StartCoroutine(SpawnEnemies(wave));
    }

    void CalculateWaveQuota(Wave wave)
    {
        int quota = 0;
        foreach (var g in wave.enemyGroups) quota += g.enemyCount;
        wave.waveQuota = quota;
    }

    IEnumerator SpawnEnemies(Wave wave)
    {
        foreach (var group in wave.enemyGroups)
        {
            for (int i = 0; i < group.enemyCount; i++)
            {
                Vector2 spawnPos = (Vector2)playerTransform.position + Random.insideUnitCircle.normalized * spawnRadius;
                Instantiate(group.enemyPrefab, spawnPos, Quaternion.identity);
                group.spawnCount++;
                wave.spawnCount++;
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }

        waveActive = false;

        // Wait before next wave
        yield return new WaitForSeconds(5f);
        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
            StartCoroutine(StartWave(currentWaveIndex));
    }
}
