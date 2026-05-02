using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentDamage;

    SpriteRenderer _sprite;
    Color _originalColor;
    float _flashTimer;
    static readonly Color HitColor = new Color(1f, 0.2f, 0.2f);

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth    = enemyData.MaxHealth;
        currentDamage    = enemyData.Damage;
        _sprite          = GetComponent<SpriteRenderer>();
        if (_sprite) _originalColor = _sprite.color;
    }

    void Update()
    {
        if (_flashTimer > 0)
        {
            _flashTimer -= Time.deltaTime;
            if (_sprite) _sprite.color = _flashTimer > 0 ? HitColor : _originalColor;
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        _flashTimer    = 0.15f;

        // Всплывающая цифра урона
        DamageNumber.Spawn(transform.position, dmg);

        // Звук попадания
        SoundManager.Instance?.PlayWeaponHit();

        if (currentHealth <= 0) Kill();
    }

    public void Kill()
    {
        FindObjectOfType<GameHUD>()?.RegisterKill();
        SoundManager.Instance?.PlayEnemyDeath();
        EnemyDeathEffect.Spawn(transform.position, _sprite?.color ?? Color.red);
        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.GetComponent<PlayerStats>()?.TakeDamage(currentDamage * Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerStats>()?.TakeDamage(currentDamage * Time.deltaTime);
    }
}
