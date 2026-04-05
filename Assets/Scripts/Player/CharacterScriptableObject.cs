using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="CharacterScriptableObject", menuName ="ScriptableObjects/Character")]
public class CharacterScriptableObject : ScriptableObject

{
    [SerializeField]
    private GameObject startingWeapon;
    public GameObject StartingWeapon { get => startingWeapon; set => startingWeapon = value; }

    [SerializeField]
    private float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    [SerializeField]
    private float hpRegen;
    public float HpRegen { get => hpRegen; set => hpRegen = value; }


    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    [SerializeField]
    private int projectileCount;
    public int ProjectileCount { get => projectileCount; set => projectileCount = value; }

    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }

    [SerializeField]
    private float magnetRadius;

    public float MagnetRadius { get => magnetRadius; set => magnetRadius = value; }
}
