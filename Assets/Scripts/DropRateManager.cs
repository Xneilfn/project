using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        [Range(0f, 100f)]
        public float dropRate;
    }

    public List<Drops> drops;

    private void OnDestroy()
    {
        // Не спавним дропы при выгрузке сцены
        if (!gameObject.scene.isLoaded) return;

        float randomNumber = Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops drop in drops)
        {
            if (drop.itemPrefab == null) continue;
            if (randomNumber <= drop.dropRate)
                possibleDrops.Add(drop);
        }

        if (possibleDrops.Count > 0)
        {
            Drops chosen = possibleDrops[Random.Range(0, possibleDrops.Count)];
            Instantiate(chosen.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
