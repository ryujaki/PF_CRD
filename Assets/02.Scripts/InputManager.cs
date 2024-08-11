using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    //public Action KeyAction = null;
    //public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;

    int layerUnit;
    int layerEnemy;
    int layerMainStage;
    int layerWisp;
    int layerWispHome;

    public void LayerSetting()
    {
        layerUnit = LayerMask.GetMask("Unit");
        layerEnemy = LayerMask.GetMask("Enemy");
        layerMainStage = LayerMask.GetMask("MainStage");
        layerWisp = LayerMask.GetMask("Wisp");
        layerWispHome = LayerMask.GetMask("WispHome");
    }         

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (!_pressed)
            {
                //처음 한번 눌렀을 때
                if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerUnit))
                {
                    Unit unit = rayHit.collider.gameObject.GetComponent<Unit>();
                    if (unit != null)
                    {
                        if (Player.Instance.selectedUnit == unit)
                            return;

                        Player.Instance.selectedUnit = unit;
                        Debug.Log("unit : " + rayHit.collider.gameObject.name);
                    }  
                }

                else if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerEnemy))
                {
                    Enemy enemy = rayHit.collider.gameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        if (Player.Instance.selectedEnemy == enemy)
                            return;

                        Player.Instance.selectedEnemy = enemy;
                        Debug.Log("enemy : " + rayHit.collider.gameObject.name);
                    }
                }

                else if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMainStage))
                {
                    if (Player.Instance.selectedUnit != null)
                    {
                        Player.Instance.selectedUnit = null;
                    }
                }

                else if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerWisp))
                {
                    Wisp wisp = rayHit.collider.gameObject.GetComponent<Wisp>();
                    if (wisp != null)
                    {
                        if (Player.Instance.selectedWisp == wisp)
                            return;

                        Player.Instance.selectedWisp = wisp;
                        Debug.Log("wisp : " + rayHit.collider.gameObject.name);
                    }
                }

                else if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerWispHome))
                {
                    if (Player.Instance.selectedWisp != null)
                    {
                        Player.Instance.selectedWisp = null;
                    }
                }

                _pressedTime = Time.time;
            }

            // 누른 상태
            _pressed = true;
        }
        else
        {
            if (_pressed)
            {
                if (Time.time < _pressedTime + 0.2f)
                {
                    // 클릭 상태                    
                }
                // 누른 상태에서 조금 있다가 땠을 때      
            }

            _pressed = false;
            _pressedTime = 0;
        }

        if (Input.GetMouseButton(1))
        {        
            if (!_pressed)
            {
                // 처음 한번 눌렀을 때 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;

                // 클릭 상태
                if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerUnit))
                {
                    if (Player.Instance.selectedUnit != null)
                    {
                        Vector3 moveToPos = rayHit.point;
                        // todo : y축 문제 해결
                        moveToPos.y = 0f;
                        Player.Instance.selectedUnit.arrivePos = moveToPos;
                        Player.Instance.selectedUnit.DoMove = true;
                    }
                }

                else if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerEnemy))
                {
                    if (Player.Instance.selectedUnit != null)
                    {
                        Enemy enemy = rayHit.collider.gameObject.GetComponent<Enemy>();
                        //Player.Instance.selectedEnemy = enemy;
                    }
                    else
                    {
                        return;
                    }
                }

                else if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMainStage))
                {
                    if (Player.Instance.selectedUnit != null)
                    {
                        Vector3 moveToPos = rayHit.point;
                        // todo : y축 문제 해결
                        moveToPos.y = 0f;
                        Player.Instance.selectedUnit.arrivePos = moveToPos;
                        Player.Instance.selectedUnit.DoMove = true;
                    }
                    else
                    {

                    }
                }

                else if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerWispHome))
                {
                    if (Player.Instance.selectedWisp != null)
                    {
                        Vector3 moveToPos = rayHit.point;
                        // todo : y축 문제 해결
                        moveToPos.y = 0f;
                        Player.Instance.selectedWisp.arrivePos = moveToPos;
                        Player.Instance.selectedWisp.DoMove = true;
                    }
                    else
                    {

                    }
                }   
                _pressedTime = Time.time;
            }

            // 누른 상태
            _pressed = true;
        }
        else
        {           
            if (_pressed)
            {
                if (Time.time < _pressedTime + 0.2f)
                {                    
                    // 누른 상태에서 조금 있다가 땠을 때
                }

                _pressed = false;
                _pressedTime = 0;
            }
        }
    }   
}
