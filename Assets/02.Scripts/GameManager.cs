using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stage
{
    public float stageTime;
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int curStageIndex = 0;
    [SerializeField] Stage[] stages;
    [SerializeField] float remainedTime;
    public string remainedTimeStr;

    public int CurStageIndex => curStageIndex + 1;
    public int MaxStage => stages.Length;

    public EnemySpawner GetEnemySpawner;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(CountDown());
    }

    public void StartStage()
    {
        curStageIndex++;
        GetEnemySpawner.Spawn();
    }

    IEnumerator CountDown()
    {
        remainedTime = 10f;
        while (remainedTime > 0)
        {
            remainedTime -= Time.deltaTime;
            //int hour = Mathf.FloorToInt(remainedTime / 3600);
            //float remainedTime2 = remainedTime % 3600;
            //int min = Mathf.FloorToInt(remainedTime2 / 60);
            //int sec = Mathf.FloorToInt(remainedTime2 % 60);
            int min = Mathf.FloorToInt(remainedTime / 60);
            int sec = Mathf.FloorToInt(remainedTime % 60);
            remainedTimeStr = string.Format("00:{0:00}:{1:00}", min, sec);
            //remainedTimeStr = string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
            yield return null;
        }
        remainedTime = 0;
        remainedTimeStr = "00:00:00";
        StartStage();
    }

}
