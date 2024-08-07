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
    [SerializeField] GameObject rangeCanvasObj;
    [SerializeField] Draggable draggable;

    Coroutine curPlayingCo;
    [SerializeField] UnitState unitState = UnitState.Idle;
    [SerializeField] EnemySpawner enemySpawner;

    private void Awake()
    {
        draggable = GetComponent<Draggable>();
        if (transform.GetChild(0) != null)
        {
            rangeCanvasObj = transform.GetChild(0).gameObject;
            rangeCanvasObj.SetActive(false);
        }
    }

    private void Update()
    {
        if (target != null && !draggable.IsClickedUnitExist)
            RotateToTarget();

        if (draggable.IsClickedUnitExist)
        {
            if (rangeCanvasObj.activeSelf == false)
                rangeCanvasObj.SetActive(true);
        }            
        else
        {
            if (rangeCanvasObj.activeSelf == true)
                rangeCanvasObj.SetActive(false);
        }
    }

    void RotateToTarget()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);
    }   

    public void SetUp(EnemySpawner _enemySpawner)
    {
        enemySpawner = _enemySpawner;
        ChangeState(UnitState.Idle);
    }

    void ChangeState(UnitState newState)
    {
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

            if (!draggable.IsClickedUnitExist)
            {
                if (target != null)
                    ChangeState(UnitState.Attack);
            }

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

            if (!draggable.IsClickedUnitExist)
            {
                EnemyHP enemyHP = target.GetComponent<EnemyHP>();
                if (enemyHP != null)
                    enemyHP.TakeDamage(attackDamage);
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }
}
