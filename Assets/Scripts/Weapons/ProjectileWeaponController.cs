using UnityEngine;

/// <summary>
/// Стреляет пулей в ближайшего врага.
/// Создай ScriptableObject оружия и prefab пули с компонентом ProjectileBehavior.
/// Прикрепи этот скрипт на Player рядом с WeaponController.
/// </summary>
public class ProjectileWeaponController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();

        // Ищем ближайшего врага
        EnemyStats nearest = FindNearestEnemy();
        if (nearest == null) return;

        Vector2 dir = (nearest.transform.position - transform.position).normalized;

        GameObject bullet = Instantiate(weaponData.Prefab, transform.position, Quaternion.identity);
        ProjectileBehavior proj = bullet.GetComponent<ProjectileBehavior>();
        if (proj != null)
        {
            proj.Init(dir, weaponData.Damage, weaponData.Pierce, weaponData.Speed);
        }
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
