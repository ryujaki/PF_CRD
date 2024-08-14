using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    GameObject wispCanvasObj;
    public Vector3 arrivePos = Vector3.zero;
    [SerializeField] bool doMove = false;

    public bool DoMove
    {
        get => doMove;
        set => doMove = value;
    }

    private void Awake()
    {
        Bind();
    }

    private void Update()
    {
        if (Player.Instance.selectedWisp == this)
        {
            if (wispCanvasObj.activeSelf == false)
                wispCanvasObj.SetActive(true);
        }
        else
        {
            if (wispCanvasObj.activeSelf == true)
                wispCanvasObj.SetActive(false);
        }

        if (doMove)
        {
            Vector3 dir = (arrivePos - transform.position).normalized;
            // todo : moveSpeed 설정
            transform.position += dir * 2f * Time.deltaTime;

            if (Vector3.Distance(transform.position, arrivePos) < 0.05f)
            {
                doMove = false;
            }
        }        
    }

    void Bind()
    {
        if (wispCanvasObj == null)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Equals("WispCanvas"))
                {
                    wispCanvasObj = child.gameObject;
                    wispCanvasObj.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DrawPortal"))
        {
            Player.Instance.SpawnUnit();
            Player.Instance.DestroyWisps(gameObject);
        }
        else if (other.CompareTag("GoldGamblingPortal"))
        {
            Player.Instance.GoldGambling();
        }
        else if (other.CompareTag("TreePortal"))
        {
            Player.Instance.PlusTreeCount(1);
        }
        else 
        {

        }


    }
}
