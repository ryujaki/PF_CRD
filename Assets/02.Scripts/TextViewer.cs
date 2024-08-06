using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextViewer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerGoldText;
    [SerializeField] TextMeshProUGUI curEnemyCountText;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] TextMeshProUGUI remainedTimeText;
    [SerializeField] Player player;
    [SerializeField] GameManager gm;
    [SerializeField] EnemySpawner enemySpawner;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gm = FindObjectOfType<GameManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void Update()
    {
        playerGoldText.text = player.CurrentGold.ToString();
        curEnemyCountText.text = $"{enemySpawner.CurMapEnemyCount}";
        stageText.text = $"{gm.CurStageIndex + 1} 라운드 시작까지";
        remainedTimeText.text = gm.remainedTimeStr;       
    }
}
