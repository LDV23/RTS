using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState
{
    UnitsSelected,
    Frame,
    Other
}

public class Management : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableObject _howered;

    private List<SelectableObject> _listOfSelected = new List<SelectableObject>();

    [SerializeField] private Image _frameImage;
    private Vector2 _frameStart, _frameEnd;

    private SelectionState _currentsSelectionState;

    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            SelectableCollider selectableCollider = hit.collider.GetComponent<SelectableCollider>();
            if (selectableCollider)
            {
                SelectableObject hitSelectable = selectableCollider.SelectableObject;
                if(_howered)
                {
                    if(_howered != hitSelectable)
                    {
                        _howered.OnUnhover();
                        _howered = hitSelectable;
                        _howered.OnHover();
                    }
                }
                else
                {
                    _howered = hitSelectable;
                    _howered.OnHover();
                }
            }
            else
            {
                UnhoverCurrent();
            }
        }
        else
        {
            UnhoverCurrent();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_howered)
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    UnselectAll();
                }

                _currentsSelectionState = SelectionState.UnitsSelected;
                Select(_howered);
            }
        }

        if(_currentsSelectionState == SelectionState.UnitsSelected)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider.tag == "Ground")
                {
                    int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(_listOfSelected.Count));
                    for (int i = 0; i < _listOfSelected.Count; i++)
                    {
                        int row = i / rowNumber;
                        int colum = i % rowNumber;

                        Vector3 point = hit.point + new Vector3(row, 0f, colum);

                        _listOfSelected[i].WhenClickOnGround(point);
                    }
                }
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            UnselectAll();
        }

        //Выделение рамкой  
        if(Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }
        if(Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector2 size = max - min;

            if(size.magnitude > 10f)
            {
                _frameImage.enabled = true;

                _frameImage.rectTransform.anchoredPosition = min;
                _frameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(min, size);

                UnselectAll();
                Unit[] allUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < allUnits.Length; i++)
                {
                    Vector2 screenPoint = _camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if (rect.Contains(screenPoint))
                    {
                        Select(allUnits[i]);
                    }
                }

                _currentsSelectionState = SelectionState.Frame;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            _frameImage.enabled = false;
            if (_listOfSelected.Count > 0)
            {
                _currentsSelectionState = SelectionState.UnitsSelected;
            }
            else
            {
                _currentsSelectionState = SelectionState.Other;
            }
        }
    }

    private void UnhoverCurrent()
    {
        if (_howered)
        {
            _howered.OnUnhover();
            _howered = null;
        }
    }

    private void Select(SelectableObject selectableObject)
    {
        if (!_listOfSelected.Contains(selectableObject))
        {
            _listOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    public void Unselect(SelectableObject selectableObject)
    {
        if(_listOfSelected.Contains(selectableObject))
        {
            _listOfSelected.Remove(selectableObject);
        }
    }

    private void UnselectAll()
    {
        for (int i = 0; i < _listOfSelected.Count; i++)
        {
            _listOfSelected[i].Unselect();
        }
        _listOfSelected.Clear();
        _currentsSelectionState = SelectionState.Other;
    }
}
