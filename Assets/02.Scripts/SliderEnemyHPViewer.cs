using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderEnemyHPViewer : MonoBehaviour
{
    EnemyHP enemyHP;
    Slider hpSlider;

    public void Setup(EnemyHP _enemyHP)
    {
        enemyHP = _enemyHP;
        hpSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        hpSlider.value = enemyHP.CurrentHP / enemyHP.MaxHP;
    }
}
