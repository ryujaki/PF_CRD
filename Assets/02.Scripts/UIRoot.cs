using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRoot : MonoBehaviour
{
    public Button MoveCamToMainStageButton;
    public Button drawingButton;
    public TextMeshProUGUI wispCountText;

    private void Awake()
    {
        MoveCamToMainStageButton.onClick.AddListener(MoveCamToMainStageButtonClick);
        drawingButton.onClick.AddListener(DrawingButtonClick);
    }

    private void Update()
    {
        wispCountText.text = Player.Instance.CurrentWisp.ToString();
    }

    void MoveCamToMainStageButtonClick()
    {
        GameManager.Instance.CameraSetting(true);
    }


    public void DrawingButtonClick()
    {
        GameManager.Instance.CameraSetting(false);
    }
}
