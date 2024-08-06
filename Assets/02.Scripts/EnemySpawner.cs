using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyHPSliderPrefab;
    [SerializeField] Transform hpCanvasTransform;
    [SerializeField] Transform[] wayPoints;
    int enemyCount = 10;        // Todo : 각 스테이지별 카운트     
    Coroutine spawnCo;
    WaitForSeconds spawnDelay = new WaitForSeconds(1f);

    [SerializeField] int currentEnemyCount = 0;
    public int CurrentEnemyCount => currentEnemyCount;

    [SerializeField] List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {        
        FindWayPoint();
        enemyList = new List<Enemy>();        
    }

    private void Start()
    {
        GameManager.Instance.GetEnemySpawner = this;
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

    public void Spawn()
    {
        spawnCo = StartCoroutine(SpawnCo());
    }

    IEnumerator SpawnCo()
    {
        while (enemyCount > 0)
        {
            GameObject enemyGo = Instantiate(enemyPrefab, transform);
            Enemy enemy = enemyGo.GetComponent<Enemy>();
            enemy.SetUp(this, wayPoints);
            enemyList.Add(enemy);
            SpawnEnemyHPSlider(enemyGo);
            currentEnemyCount++;
            enemyCount--;

            yield return spawnDelay;
        }
    }

    public void DestoryEnemy(Enemy enemy, int gold)
    {
        Player.Instance.CurrentGold += gold;

        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        sliderClone.transform.SetParent(hpCanvasTransform);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<SliderAutoSetter>().Setup(enemy.transform);
        sliderClone.GetComponent<SliderEnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
