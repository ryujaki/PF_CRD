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
    public int wispCount;
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int curStageIndex = -1;
    [SerializeField] Stage[] stages;
    [SerializeField] float remainedTime;
    public string remainedTimeStr;

    public int CurStageIndex => curStageIndex + 1;
    public int MaxStage => stages.Length;

    public EnemySpawner GetEnemySpawner;

    public Transform camPos0;
    public Transform camPos1;

    Camera mainCam;

    protected override void Awake()
    {
        base.Awake();
        mainCam = Camera.main;
}

    private void Start()
    {
        CameraSetting(true);
        Player.Instance.SpawnWisps();
        //Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(CountDown(10));
    }

    public void StartStage()
    {
        if (curStageIndex < stages.Length - 1)
        {
            curStageIndex++;
            Player.Instance.CurrentWisp += stages[curStageIndex].wispCount;
            Player.Instance.SpawnWisps();
            GetEnemySpawner.Spawn(stages[curStageIndex]);
            StartCoroutine(CountDown(stages[curStageIndex].stageTime));
        }        
    }

    IEnumerator CountDown(float stageTime)
    {
        remainedTime = stageTime;
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
        remainedTimeStr = "00:00:00";
        remainedTime = 0;
        StartStage();
    }

    public void CameraSetting(bool isMainStage)
    {
        if (isMainStage)
        {
            mainCam.transform.SetLocalPositionAndRotation(camPos0.transform.position, camPos0.transform.rotation);
        }
        else
        {
            mainCam.transform.SetLocalPositionAndRotation(camPos1.transform.position, camPos1.transform.rotation);
        }
    }

}
