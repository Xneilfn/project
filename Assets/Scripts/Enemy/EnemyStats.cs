using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth    = enemyData.MaxHealth;
        currentDamage    = enemyData.Damage;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) Kill();
    }

    public void Kill()
    {
        FindObjectOfType<GameHUD>()?.RegisterKill();
        Destroy(gameObject);
    }

    // Работает когда коллайдер врага — Trigger
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerStats player = col.GetComponent<PlayerStats>();
            player?.TakeDamage(currentDamage * Time.deltaTime);
        }
    }

    // Работает когда коллайдер врага — обычный (не Trigger)
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player?.TakeDamage(currentDamage * Time.deltaTime);
        }
    }
}
