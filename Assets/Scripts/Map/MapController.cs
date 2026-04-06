using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius = 1f;
    public LayerMask terrainMask;
    public GameObject currentChunk;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    public float maxOptimizationDistance = 30f;
    public float optimizerCooldownDuration = 1f;

    Player _pl;
    float _optimizerCooldown;
    Vector3 _noTerrainPosition;

    // Все 8 направлений для проверки
    static readonly string[] Directions = {
        "Top", "Bot", "Left", "Right",
        "Top Right", "Top Left", "Bot Right", "Bot Left"
    };

    void Start()
    {
        _pl = FindObjectOfType<Player>();
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunk) return;

        // Проверяем все 8 направлений независимо от движения игрока
        foreach (string dir in Directions)
        {
            Transform point = currentChunk.transform.Find(dir);
            if (point == null) continue;

            if (!Physics2D.OverlapCircle(point.position, checkerRadius, terrainMask))
            {
                _noTerrainPosition = point.position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        GameObject chunk = Instantiate(terrainChunks[rand], _noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(chunk);
    }

    void ChunkOptimizer()
    {
        _optimizerCooldown -= Time.deltaTime;
        if (_optimizerCooldown > 0) return;
        _optimizerCooldown = optimizerCooldownDuration;

        foreach (GameObject chunk in spawnedChunks)
        {
            if (chunk == null) continue;
            float dist = Vector3.Distance(_pl.transform.position, chunk.transform.position);
            chunk.SetActive(dist <= maxOptimizationDistance);
        }
    }
}
