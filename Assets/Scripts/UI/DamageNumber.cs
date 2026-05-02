using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Всплывающая цифра урона над врагом.
/// Вызывай DamageNumber.Spawn(position, damage) из EnemyStats.TakeDamage().
/// </summary>
public class DamageNumber : MonoBehaviour
{
    TextMeshPro _text;
    float _timer;
    Vector3 _velocity;

    public static void Spawn(Vector3 position, float damage)
    {
        // Ищем Canvas в мире или создаём объект прямо в мировых координатах
        GameObject go = new GameObject("DamageNumber");
        go.transform.position = position + Vector3.up * 0.3f;
        go.AddComponent<DamageNumber>().Init(damage);
    }

    void Init(float damage)
    {
        _text = gameObject.AddComponent<TextMeshPro>();
        _text.text      = Mathf.CeilToInt(damage).ToString();
        _text.fontSize  = damage >= 50 ? 4f : 3f;          // крит — крупнее
        _text.color     = damage >= 50 ? Color.yellow : Color.white;
        _text.fontStyle = FontStyles.Bold;
        _text.alignment = TextAlignmentOptions.Center;
        _text.sortingOrder = 20;

        // Случайное направление вверх
        _velocity = new Vector3(Random.Range(-0.5f, 0.5f), 1.5f, 0f);
        _timer = 0.8f;

        Destroy(gameObject, 1f);
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
        _velocity          *= 0.9f; // затухание

        // Плавное исчезновение
        if (_text)
        {
            Color c = _text.color;
            c.a = Mathf.Clamp01(_timer / 0.8f);
            _text.color = c;
        }
    }
}
