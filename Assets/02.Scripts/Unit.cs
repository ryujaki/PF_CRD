using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState { Idle, Attack}

public class Unit : MonoBehaviour
{
    [SerializeField] int attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackDelay;
    [SerializeField] Transform target;
    [SerializeField] bool isClicked = false;
    public GameObject rangePrefab;

    [SerializeField] Draggable draggable;

    Coroutine curPlayingCo;
    [SerializeField] UnitState unitState = UnitState.Idle;
    [SerializeField] EnemySpawner enemySpawner;

    private void Awake()
    {
        draggable = GetComponent<Draggable>();
    }

    private void Update()
    {
        if (target != null)
            RotateToTarget();

        if (draggable.IsClickedUnitExist && !isClicked)
        { 
            isClicked = true;
            //GameObject go = Instantiate(rangePrefab, transform);
            //go.transform.localScale = new Vector3(attackRange, 0.01f, attackRange);

        }

        if (target != null)
        {
            Debug.Log("거리 : " + Vector3.Distance(transform.position, target.transform.position));
        }
    }

    void RotateToTarget()
    {
        float dx = target.position.x - transform.position.x;
        float dy = target.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }   

    public void SetUp(EnemySpawner _enemySpawner)
    {
        enemySpawner = _enemySpawner;
        ChangeState(UnitState.Idle);
    }

    void ChangeState(UnitState newState)
    {
        //StopCoroutine(unitState.ToString());
        //unitState = newState;
        //StartCoroutine(unitState.ToString());

        if ( curPlayingCo != null)
            StopCoroutine(curPlayingCo);
        unitState = newState;
        curPlayingCo = StartCoroutine(unitState.ToString());
    }

    IEnumerator Idle()
    {
        while (true)
        {
            float closestDistSqr = Mathf.Infinity;
            for (int i = 0; i < enemySpawner.EnemyList.Count; i++)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

                if (distance <= attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    target = enemySpawner.EnemyList[i].transform;
                }
            }

            if (target != null)
                ChangeState(UnitState.Attack);

            yield return null;
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (target == null)
            {
                ChangeState(UnitState.Idle);
                break;
            }

            float distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance > attackRange)
            {
                target = null;
                ChangeState(UnitState.Idle);
                break;
            }

            // draggable
            if (!draggable.IsOnDrag)
            {
                EnemyHP enemyHP = target.GetComponent<EnemyHP>();
                if (enemyHP != null)
                    enemyHP.TakeDamage(attackDamage);
            }
            
            yield return new WaitForSeconds(attackDelay);
        }
    }
}
