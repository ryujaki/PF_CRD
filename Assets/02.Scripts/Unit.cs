using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState { Idle, Attack}

// 참고 : attackRange 1당 scale 0.24로 계산

public class Unit : MonoBehaviour
{    
    [SerializeField] int attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackDelay;
    [SerializeField] Transform target;
    [SerializeField] GameObject rangeCanvasObj;
    [SerializeField] Draggable draggable;
    
    public Vector3 arrivePos = Vector3.zero;

    [SerializeField] UnitState unitState = UnitState.Idle;
    [SerializeField] EnemySpawner enemySpawner;

    [SerializeField] bool doMove = false;
    public bool DoMove
    {
        get => doMove;
        set => doMove = value;
    }

    private void Awake()
    {
        if (transform.GetChild(0) != null)
        {
            rangeCanvasObj = transform.GetChild(0).gameObject;
            rangeCanvasObj.SetActive(false);
        }
    }

    private void Update()
    {
        if (target != null && !doMove)
            RotateToTarget();

        if (Player.Instance.selectedUnit == this)
        {
            if (rangeCanvasObj.activeSelf == false)
                rangeCanvasObj.SetActive(true);
        }
        else
        {
            if (rangeCanvasObj.activeSelf == true)
                rangeCanvasObj.SetActive(false);
        }

        if (doMove)
        {
            target = null;
            Vector3 dir = (arrivePos - transform.position).normalized;
            // todo : moveSpeed 설정
            transform.position += dir * 2f * Time.deltaTime;

            if (Vector3.Distance(transform.position, arrivePos) < 0.05f)
            {
                doMove = false;
            }
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
        StopCoroutine(unitState.ToString());
        unitState = newState;
        StartCoroutine(unitState.ToString());
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

            if (!doMove)
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

            if (!doMove)
            {
                EnemyHP enemyHP = target.GetComponent<EnemyHP>();
                if (enemyHP != null)
                    enemyHP.TakeDamage(attackDamage);
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }
}
