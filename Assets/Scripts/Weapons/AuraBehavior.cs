using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AuraBehavior : MeleeWeaponBehavior
{
    List<GameObject> markedEnemies = new List<GameObject>();
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!markedEnemies.Contains(collision.gameObject) && collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            markedEnemies.Add(collision.gameObject);

        }
    }

}
