using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [Tooltip("The camera used for highlighting")]
    public Camera mainCam;
    [Tooltip("The rectangle to modify for selection")]
    public RectTransform SelectingBoxRect;

    private Rect SelectingRect;
    private Vector3 SelectingStart;

    [Tooltip("Changes the minimum square before selecting characters. Needed for single click select")]
    public float minBoxSizeBeforeSelect = 10f;
    public float selectUnderMouseTimer = 0.1f;
    private float selectTimer = 0f;

    private bool selecting = false;


    private void Awake()
    {
        mainCam = Camera.main;

        //This assumes that the manager is placed on the image used to select
        if (!SelectingBoxRect)
        {
            SelectingBoxRect = GetComponent<RectTransform>();
        }
    }

    void Update()
    {
        if (SelectingBoxRect == null)
        {
            Debug.LogError("There is no Rect Transform to use for selection!");
            return;
        }

        //The input for triggering selecting. This can be changed
        if (Input.GetMouseButtonDown(0))
        {
            ReSelect();

            //Sets up the screen box
            SelectingStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            SelectingBoxRect.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectTimer = 0f;
        }

        selecting = Input.GetMouseButton(0);

        if (selecting)
        {
            SelectingUnits();
            selectTimer += Time.deltaTime;
           
        }
        else
            SelectingBoxRect.sizeDelta = new Vector2(0, 0);
    }

    //Resets what is currently being selected
    void ReSelect()
    {
        for (int i = 0; i < Player.Instance.selectedUnits.Count; i++)
        {
            Player.Instance.selectedUnits[i].UnitCanvasActivate(false);
            Player.Instance.selectedUnits.Remove(Player.Instance.selectedUnits[i]);
        }
        
    }

    //Does the calculation for mouse dragging on screen
    //Moves the UI pivot based on the direction the mouse is going relative to where it started
    //Update: Made this a bit more legible
    void SelectingUnits()
    {
        Vector2 _pivot = Vector2.zero;
        Vector3 _sizeDelta = Vector3.zero;
        Rect _rect = Rect.zero;

        //Controls x's of the pivot, sizeDelta, and rect
        if (-(SelectingStart.x - Input.mousePosition.x) > 0)
        {
            _sizeDelta.x = -(SelectingStart.x - Input.mousePosition.x);
            _rect.x = SelectingStart.x;
        }
        else
        {
            _pivot.x = 1;
            _sizeDelta.x = (SelectingStart.x - Input.mousePosition.x);
            _rect.x = SelectingStart.x - SelectingBoxRect.sizeDelta.x;
        }

        //Controls y's of the pivot, sizeDelta, and rect
        if (SelectingStart.y - Input.mousePosition.y > 0)
        {
            _pivot.y = 1;
            _sizeDelta.y = SelectingStart.y - Input.mousePosition.y;
            _rect.y = SelectingStart.y - SelectingBoxRect.sizeDelta.y;
        }
        else
        {
            _sizeDelta.y = -(SelectingStart.y - Input.mousePosition.y);
            _rect.y = SelectingStart.y;
        }

        //Sets pivot if of UI element
        if (SelectingBoxRect.pivot != _pivot)
            SelectingBoxRect.pivot = _pivot;

        //Sets the size
        SelectingBoxRect.sizeDelta = _sizeDelta;

        //Finished the Rect set up then set rect
        _rect.height = SelectingBoxRect.sizeDelta.y;
        _rect.width = SelectingBoxRect.sizeDelta.x;
        SelectingRect = _rect;

        //Only does a select check if the box is bigger than the minimum size.
        //While checking it messes with single click
        if (_rect.height > minBoxSizeBeforeSelect && _rect.width > minBoxSizeBeforeSelect)
        {
            CheckForSelectedUnits();
        }
    }

    //Checks if the correct characters can be selected and then "selects" them
    void CheckForSelectedUnits()
    {
        foreach (Unit unit in Player.Instance.spawnedUnits)
        {
            Vector2 screenPos = mainCam.WorldToScreenPoint(unit.transform.position);
            if (SelectingRect.Contains(screenPos))
            {
                if (!Player.Instance.selectedUnits.Contains(unit))
                {
                    Player.Instance.selectedUnits.Add(unit);
                }

                unit.UnitCanvasActivate(true);
            }
            else if (!SelectingRect.Contains(screenPos))
            {
                unit.UnitCanvasActivate(false);

                if (Player.Instance.selectedUnits.Contains(unit))
                {
                    Player.Instance.selectedUnits.Remove(unit);
                }
            }
        }
    }
}
