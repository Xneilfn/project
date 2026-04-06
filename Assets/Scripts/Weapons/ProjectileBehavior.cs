using UnityEngine;

/// <summary>
/// Прикрепи на prefab пули.
/// Нужны: Rigidbody2D, CircleCollider2D (Is Trigger = true), SpriteRenderer.
/// </summary>
public class ProjectileBehavior : MonoBehaviour
{
    float _damage;
    int   _pierce;
    float _speed;
    Vector2 _dir;
    Rigidbody2D _rb;

    public void Init(Vector2 direction, float damage, int pierce, float speed)
    {
        _dir    = direction;
        _damage = damage;
        _pierce = pierce;
        _speed  = speed;

        // Поворачиваем спрайт в направлении полёта
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Destroy(gameObject, 5f); // самоуничтожение через 5 сек
    }

    void Awake() => _rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        if (_rb != null)
            _rb.linearVelocity = _dir * _speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;

        EnemyStats enemy = col.GetComponent<EnemyStats>();
        if (enemy != null) enemy.TakeDamage(_damage);

        _pierce--;
        if (_pierce < 0) Destroy(gameObject);
    }
}
