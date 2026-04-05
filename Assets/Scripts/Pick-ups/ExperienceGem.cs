using UnityEngine;

public class ExperienceGem : Pickup, ICollectable
{
    public int experienceGranted = 20;

    bool _collected = false;

    // Вызывается PlayerCollector через интерфейс ICollectable
    public void Collect()
    {
        if (_collected) return;
        _collected = true;

        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player != null)
            player.IncreaseExperience(experienceGranted);

        Destroy(gameObject);
    }

    // Прямой подбор — игрок касается без магнита
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            Collect();
    }
}
