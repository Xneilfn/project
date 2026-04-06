using UnityEngine;

/// <summary>
/// Прикрепи на prefab бумеранга.
/// Нужны: Rigidbody2D, CircleCollider2D (Is Trigger = true).
/// </summary>
public class BoomerangBehavior : MonoBehaviour
{
    float _damage;
    float _speed;
    Transform _owner;
    Vector2 _dir;
    bool _returning;
    float _maxDistance = 5f;
    Vector3 _startPos;

    public void Init(Vector2 direction, float damage, float speed, Transform owner)
    {
        _dir      = direction;
        _damage   = damage;
        _speed    = speed;
        _owner    = owner;
        _startPos = transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Destroy(gameObject, 6f);
    }

    void Update()
    {
        if (_owner == null) { Destroy(gameObject); return; }

        if (!_returning)
        {
            // Летим вперёд и вращаемся
            transform.position += (Vector3)_dir * _speed * Time.deltaTime;
            transform.Rotate(0, 0, 720 * Time.deltaTime);

            if (Vector3.Distance(transform.position, _startPos) >= _maxDistance)
                _returning = true;
        }
        else
        {
            // Возвращаемся к игроку
            Vector3 toOwner = (_owner.position - transform.position).normalized;
            transform.position += toOwner * _speed * 1.5f * Time.deltaTime;
            transform.Rotate(0, 0, 720 * Time.deltaTime);

            if (Vector3.Distance(transform.position, _owner.position) < 0.5f)
                Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;
        EnemyStats enemy = col.GetComponent<EnemyStats>();
        enemy?.TakeDamage(_damage);
    }
}
