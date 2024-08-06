using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints;
    [SerializeField] int curIndex = 0;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] int gold = 10;
    Patrolable patrolable;
    Coroutine patrolCo;

    public void SetUp(EnemySpawner _enemySpawner, Transform[] _wayPoints)
    {
        patrolable = GetComponent<Patrolable>();
        enemySpawner = _enemySpawner;
        wayPoints = new Transform[_wayPoints.Length];
        wayPoints = _wayPoints;

        Patrol();
    }    

    void Patrol()
    {
        transform.position = wayPoints[curIndex].position;
        Vector3 firstRot = (wayPoints[curIndex + 1].position - transform.position);
        transform.rotation = Quaternion.LookRotation(firstRot);
        patrolCo = StartCoroutine(PatrolCo());
    }

    IEnumerator PatrolCo()
    {
        NextPatrol();

        while (true)
        {
            if (Vector3.Distance(transform.position, wayPoints[curIndex].position) < 0.02f * patrolable.MoveSpeed)
            {
                NextPatrol();
            }

            yield return null;
        }
    }

    void NextPatrol()
    {
        transform.position = wayPoints[curIndex].position;
        curIndex++;
        if (curIndex == wayPoints.Length)
            curIndex = 0;
        Vector3 dir = (wayPoints[curIndex].position - transform.position).normalized;
        patrolable.MoveTo(dir);
    }

    public void Die()
    {
        enemySpawner.DestoryEnemy(this, gold);
    }
}
