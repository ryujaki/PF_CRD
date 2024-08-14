using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] Unit[] unitPrefabs;
    public List<Unit> spawnedUnits;
    [SerializeField] GameObject wispPrefab;
    [SerializeField] List<GameObject> wisps;
    [SerializeField] int currentGold = 100;
    [SerializeField] int currentWisp = 2;
    [SerializeField] int currentTree = 0;
    [SerializeField] Transform wispSpawnPos;
    public Unit selectedUnit;
    public Enemy selectedEnemy;
    public Wisp selectedWisp;

    public List<Unit> selectedUnits = new List<Unit>();

    public int CurrentGold
    {
        get => currentGold;
        set => currentGold = Mathf.Max(0, value);
    }    

    public int CurrentWisp
    {
        get => currentWisp;
        set => currentWisp = Mathf.Max(0, value);
    }

    public int CurrentTree
    {
        get => currentTree;
        set => currentTree = Mathf.Max(0, value);
    }

    InputManager input = new InputManager();
    public static InputManager Input { get { return Input; } }


    protected override void Awake()
    {
        base.Awake();
        FindSpawnPos();
        GetUnits();
        LayerSetting();
        spawnedUnits = new List<Unit>();
        wisps = new List<GameObject>();
    }

    private void Update()
    {
        input.OnUpdate();
    }

    void FindSpawnPos()
    {
        wispSpawnPos = GameObject.Find("WispSpawnPos").transform;
    }

    void GetUnits()
    {
        unitPrefabs = Resources.LoadAll<Unit>("Units");
    }

    void LayerSetting()
    {
        input.LayerSetting();
    }

    public void SpawnUnit()
    {
        if (currentWisp > 0)
        {
            int randomIndex = Random.Range(0, unitPrefabs.Length);
            //Unit unit = Instantiate(unitPrefabs[randomIndex], unitSpawnPos);
            Unit unit = Instantiate(unitPrefabs[randomIndex]);
            spawnedUnits.Add(unit);
            unit.SetUp(GameManager.Instance.GetEnemySpawner);
            currentWisp--;
        }        
    }

    public void DestroyUnit(Unit unit)
    {
        spawnedUnits.Remove(unit);
        Destroy(unit);
    }

    public void SpawnWisps()
    {
        for (int i = 0; i < currentWisp; i++)
        {
            GameObject wispObj = Instantiate(wispPrefab, wispSpawnPos.position, Quaternion.identity);
            wisps.Add(wispObj);
        }
    }

    public void DestroyWisps(GameObject wispObj)
    {
        Destroy(wispObj);
        wisps.Remove(wispObj);
    }

    public void GoldGambling()
    {
        int randomGold = Random.Range(1, GameManager.Instance.CurStageIndex + 20);
        currentGold += randomGold;
    }

    public void PlusTreeCount(int treeCount)
    {
        currentTree += treeCount;
    }
}
