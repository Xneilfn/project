using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Базовый класс, от которого наследуются все оружия в игре
public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected Player player;

    protected virtual void Start()
    {
        player = GetComponent<Player>();
        currentCooldown = weaponData.CooldownDuration;
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack();


        }
    }
    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }

}
