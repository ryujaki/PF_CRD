using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderAutoSetter : MonoBehaviour
{
    [SerializeField] Vector3 distance;
    Transform targetTransform;
    RectTransform rectTransform;

    private void Awake()
    {
        InitSetting();
    }

    void InitSetting()
    {
        distance = Vector3.up * 50f;
    }

    public void Setup(Transform target)
    {
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance;
    }
}
