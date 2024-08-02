using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolable : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.0f;
    [SerializeField] float rotateSpeed = 5f;
    [SerializeField] Vector3 moveDirection = Vector3.zero;

    public float MoveSpeed => moveSpeed;

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateSpeed);
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
