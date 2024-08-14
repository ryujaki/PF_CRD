using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCanvas : MonoBehaviour
{
    string selectionEffect = "SelectionEffect";
    [SerializeField] SpriteRenderer selectionEffectRenderer;
    string rangeIndicator = "RangeIndicator";
    [SerializeField] Image rangeIndicatorImage;

    private void Awake()
    {
        Bind();
        gameObject.SetActive(false);
    }

    void Bind()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name.Equals(selectionEffect))
            {
                selectionEffectRenderer = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
                continue;
            }

            if (transform.GetChild(i).gameObject.name.Equals(rangeIndicator))
            {
                rangeIndicatorImage = transform.GetChild(i).gameObject.GetComponent<Image>();
                RangeIndicatorEnable(false);
                continue;
            }
        }
    }

    public void RangeIndicatorEnable(bool value) => rangeIndicatorImage.enabled = value;
}
