using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [Tooltip("Скорость притяжения гемов к игроку")]
    public float pullSpeed = 5f;

    CircleCollider2D _col;
    PlayerStats _player;

    void Start()
    {
        _col    = GetComponent<CircleCollider2D>();
        _player = FindObjectOfType<PlayerStats>();

        // Начальный радиус из ScriptableObject персонажа
        // Если 0 — магнит выключен, гемы подбираются только наступив
        if (_col != null)
            _col.radius = _player.currentMagnetRadius;
    }

    void Update()
    {
        // Обновляем радиус только если он изменился (апгрейд магнита)
        if (_col != null && !Mathf.Approximately(_col.radius, _player.currentMagnetRadius))
            _col.radius = _player.currentMagnetRadius;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Магнит работает только если радиус > 0
        if (_player.currentMagnetRadius <= 0) return;

        if (collision.gameObject.TryGetComponent(out ICollectable collectable))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (transform.position - collision.transform.position).normalized;
                rb.AddForce(dir * pullSpeed);
            }
            collectable.Collect();
        }
    }
}
