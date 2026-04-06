using UnityEngine;

public class ExperienceGem : Pickup, ICollectable
{
    public int experienceGranted = 20;
    bool _collected = false;

    public void Collect()
    {
        if (_collected) return;
        _collected = true;
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player != null)
            player.IncreaseExperience(experienceGranted);
        Destroy(gameObject);
    }

    // Подбирается ТОЛЬКО когда игрок наступает на гем (OnTriggerStay2D)
    // OnTriggerEnter2D убран — он срабатывал при входе в большую зону
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            Collect();
    }
}
