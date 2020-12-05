﻿using System;
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
    [SerializeField] private Material defaultSelectedMaterial;
    [SerializeField] private Canvas canvas;

    RaycastHit hit;
    private Transform _selection;
    private GameObject _selectedObject;  // 중력스킬을 적용할 오브젝트
    private AdaptGravity gravityObject;
    private Color _defaultColor;


    private SelecMode _selecMode = SelecMode.NonSelecting;
    [SerializeField] private PlayerController player;

    private void Awake()
    {
        _defaultColor = defaultSelectedMaterial.GetColor("_EmissionColor");
        selectedMaterial.SetColor("_EmissionColor",_defaultColor);
        canvas.enabled = false;
    }

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
            canvas.enabled = false;
            if (player.SelectMode)
                _selecMode = SelecMode.Selecting;

        }
        else if (_selecMode == SelecMode.Selecting)
        {
            canvas.enabled = true ;
            if (!player.SelectMode)
                _selecMode = SelecMode.NonSelecting;
            Vector3 ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            var ray = Camera.main.ScreenPointToRay(ScreenCenter);
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
                        _selectedObject = _selection.gameObject;
                        _selectedObject.GetComponent<Renderer>().material = selectedMaterial;
                        gravityObject = _selectedObject.GetComponent<AdaptGravity>();
                        player.SelectMode = false;
                        _selection = null;
                        _selecMode = SelecMode.Selected;
                    }
                }
            }

        }else if (_selecMode == SelecMode.Selected)
        {
            canvas.enabled = false;
            if (Input.GetButtonDown("Left Click"))
            {
                _selectedObject.GetComponent<Renderer>().material = defaultMaterial;
                _selectedObject = null;
                _selecMode = SelecMode.NonSelecting;
                gravityObject.Selected = false;
                gravityObject.AdaptChanges();
                return;
            }
            
            // 마우스 휠 스크롤에 따른 중력 & UI 조정
            var axis = Input.GetAxis("Mouse ScrollWheel");
            if (axis != 0f)
            {
                gravityObject.Selected = true;
                gravityObject.AddGravity(axis);
            }

            // UI
            var color = selectedMaterial.GetColor("_EmissionColor");
            if (axis > 0)
            {
                // r -> b
                color.r -= 0.1f;
                color.g -= 0.1f;
                color.b += 0.1f;
            }else if (axis < 0)
            {
                // b -> r
                color.r += 0.1f;
                color.g -= 0.1f;
                color.b -= 0.1f;
            }

            if (gravityObject.tempAddedGravity == 0f)
            {
                color = _defaultColor;
            }
            selectedMaterial.SetColor("_EmissionColor",color);
        }
    }
}