using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints;
    [SerializeField] int curIndex = 0;
    Coroutine patrolCo;
    Patrolable patrolable;

    private void Awake()
    {
        FindWayPoint();
        patrolable = GetComponent<Patrolable>();
    }

    private void Start()
    {
        Patrol();
    }

    void FindWayPoint()
    {
        GameObject wayPointParentGo = GameObject.Find("[WayPoint]");        
        Transform wayPointParent = wayPointParentGo.transform;
        wayPoints = new Transform[wayPointParent.childCount];
        for (int i = 0; i < wayPointParent.childCount; i++)
        {
            wayPoints[i] = wayPointParent.GetChild(i);
        }
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
}
