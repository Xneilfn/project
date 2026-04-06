using UnityEngine;

/// <summary>
/// Бросает бумеранг в ближайшего врага — он летит туда и возвращается назад.
/// </summary>
public class BoomerangController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();

        EnemyStats nearest = FindNearestEnemy();
        Vector2 dir = nearest != null
            ? (nearest.transform.position - transform.position).normalized
            : Vector2.up;

        GameObject boom = Instantiate(weaponData.Prefab, transform.position, Quaternion.identity);
        BoomerangBehavior b = boom.GetComponent<BoomerangBehavior>();
        if (b != null)
            b.Init(dir, weaponData.Damage, weaponData.Speed, transform);
    }

    EnemyStats FindNearestEnemy()
    {
        EnemyStats[] enemies = FindObjectsOfType<EnemyStats>();
        EnemyStats nearest = null;
        float minDist = float.MaxValue;
        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < minDist) { minDist = d; nearest = e; }
        }
        return nearest;
    }
}
