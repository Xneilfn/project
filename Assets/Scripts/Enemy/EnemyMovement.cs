using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform PlayerLocation;


    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        PlayerLocation = FindObjectOfType<Player>().transform;
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, PlayerLocation.position, enemy.currentMoveSpeed * Time.deltaTime);
        if (PlayerLocation.position.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
