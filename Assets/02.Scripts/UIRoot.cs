using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{
    public Button drawingButton;    

    private void Awake()
    {
        drawingButton.onClick.AddListener(DrawingButtonClick);
    }

    public void DrawingButtonClick()
    {
        Player.Instance.SpawnUnit();
    }
}
