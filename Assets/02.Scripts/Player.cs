using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] Unit[] unitPrefabs;
    [SerializeField] List<Unit> units;
    [SerializeField] int currentGold = 100;
    [SerializeField] Transform unitSpawnPos;

    public int CurrentGold
    {
        get => currentGold;
        set => currentGold = Mathf.Max(0, value);
    }    

    protected override void Awake()
    {
        base.Awake();
        FindSpawnPos();
        GetUnits();
    }

    void FindSpawnPos()
    {
        unitSpawnPos = GameObject.Find("UnitSpawnPos").transform;
    }

    void GetUnits()
    {
        unitPrefabs = Resources.LoadAll<Unit>("Units");
    }


    public void SpawnUnit()
    {
        int randomIndex = Random.Range(0, unitPrefabs.Length);
        Unit unit = Instantiate(unitPrefabs[randomIndex], unitSpawnPos);
        units.Add(unit);
        unit.SetUp(GameManager.Instance.GetEnemySpawner);
    }

    public void DestroyUnit(Unit unit)
    {
        units.Remove(unit);
        Destroy(unit);
    }
}
