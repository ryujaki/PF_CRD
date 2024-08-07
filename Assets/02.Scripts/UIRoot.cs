using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRoot : MonoBehaviour
{
    public Button drawingButton;
    public TextMeshProUGUI wispCountText;

    private void Awake()
    {
        drawingButton.onClick.AddListener(DrawingButtonClick);
    }

    private void Update()
    {
        wispCountText.text = Player.Instance.CurrentWisp.ToString();
    }

    public void DrawingButtonClick()
    {
        Player.Instance.SpawnUnit();
    }
}
