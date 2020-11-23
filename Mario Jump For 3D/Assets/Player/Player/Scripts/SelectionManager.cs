using System.Collections;
using System.Collections.Generic;
using Player.Scripts;
using UnityEngine;
enum SelecMode
{
    NonSelecting,
    Selecting,
    Selected
}

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material highlightMaterial;

    RaycastHit hit;
    private Transform _selection;
    private GameObject _selectedObject;            // 중력스킬을 적용할 오브젝트

    private SelecMode _selecMode = SelecMode.NonSelecting;
    [SerializeField] private PlayerController player;

    private void Update()
    {
        if (_selection != null && _selecMode != SelecMode.Selected)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }

        if (_selecMode == SelecMode.NonSelecting)
        {
            if (player.SelectMode)
                _selecMode = SelecMode.Selecting;

        }
        else if (_selecMode == SelecMode.Selecting)
        {
            if (!player.SelectMode)
                _selecMode = SelecMode.NonSelecting;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;
                if (selection.CompareTag(selectableTag))
                {
                    Debug.Log("Selecting");
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        defaultMaterial = selectionRenderer.material;
                        selectionRenderer.material = highlightMaterial;
                    }

                    _selection = selection;
                    if (Input.GetButtonDown("Left Click"))
                    {
                        Debug.Log("Select");
                        _selectedObject = _selection.gameObject;
                        _selectedObject.GetComponent<Renderer>().material = selectedMaterial;
                        player.SelectMode = false;
                        _selection = null;
                        _selecMode = SelecMode.Selected;
                    }
                }
            }

        }else if (_selecMode == SelecMode.Selected)
        {
            if (Input.GetButtonDown("Left Click"))
            {
                _selectedObject.GetComponent<Renderer>().material = defaultMaterial;
                _selectedObject = null;
                _selecMode = SelecMode.NonSelecting;

            }
            // 마우스 휠 스크롤에 따른 중력 & UI 조정
            
            
        }
    }



}