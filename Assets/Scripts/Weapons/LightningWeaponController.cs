using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Молния — мгновенно бьёт ближайшего врага и перепрыгивает на следующих.
/// Pierce в WeaponScriptableObject = количество прыжков.
/// Не требует prefab снаряда — визуал через LineRenderer.
/// </summary>
public class LightningWeaponController : WeaponController
{
    [Header("Lightning")]
    [Tooltip("Максимальная дистанция прыжка между врагами")]
    public float chainRange = 5f;

    LineRenderer _line;

    protected override void Start()
    {
        base.Start();
        // LineRenderer для визуала молнии
        _line = gameObject.AddComponent<LineRenderer>();
        _line.startWidth   = 0.05f;
        _line.endWidth     = 0.05f;
        _line.material     = new Material(Shader.Find("Sprites/Default"));
        _line.startColor   = Color.cyan;
        _line.endColor     = new Color(0.5f, 0f, 1f);
        _line.positionCount = 0;
        _line.useWorldSpace = true;
    }

    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(LightningChain());
    }

    IEnumerator LightningChain()
    {
        List<EnemyStats> hit = new List<EnemyStats>();
        List<Vector3> points = new List<Vector3> { transform.position };

        EnemyStats current = FindNearest(transform.position, hit);
        int jumps = weaponData.Pierce + 1;

        for (int i = 0; i < jumps && current != null; i++)
        {
            current.TakeDamage(weaponData.Damage);
            hit.Add(current);
            points.Add(current.transform.position);
            current = FindNearest(current.transform.position, hit);
        }

        // Показываем молнию
        _line.positionCount = points.Count;
        _line.SetPositions(points.ToArray());

        yield return new WaitForSeconds(0.1f);
        _line.positionCount = 0;
    }

    EnemyStats FindNearest(Vector3 from, List<EnemyStats> exclude)
    {
        EnemyStats nearest = null;
        float minDist = chainRange;
        foreach (var e in FindObjectsOfType<EnemyStats>())
        {
            if (exclude.Contains(e)) continue;
            float d = Vector3.Distance(from, e.transform.position);
            if (d < minDist) { minDist = d; nearest = e; }
        }
        return nearest;
    }
}
