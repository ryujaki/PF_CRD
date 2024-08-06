using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] float maxHP;
    float currentHP;
    bool isDie = false;
    Enemy enemy;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(float damage)
    {
        if (isDie == true)
            return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            isDie = true;
            enemy.Die();
        }
    }
}
