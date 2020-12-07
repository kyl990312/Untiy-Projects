using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using Object = System.Object;

public class AdaptGravity : MonoBehaviour
{
   private Rigidbody _rigidbody;

   private float _addedGravity;
   private float _tempAddedGravity;
   [SerializeField] private float gravity = 9.8f;
   [SerializeField] private float power = 10f;


    private Color _defaultColor;
   private float _prevAxis = 0f;
    

   private float _sec;
   public float maxSec = 15f;

   private float _velocity;
   private float _defaultY;
   private float _minY;

    private bool _selecting;

    // UI
    private Renderer _renderer;
    private Material _defaultMat;
    private Material _selectingMat;
    private Material _defaultSelectingMat;

    void Awake()
   {
      _rigidbody = GetComponent<Rigidbody>();
      _defaultY = transform.position.y;
      Transform[] transforms = GetComponentsInChildren<Transform>();
      if (transforms[1] != null)
      {
         _minY = transforms[1].position.y;
         Destroy(transforms[1].gameObject);
      }
        _renderer = GetComponent<Renderer>();
        if(_renderer == null)
        {
            Debug.LogError("Renderer Not Loaded!");
            return;
        }
        _defaultMat = _renderer.material;
    }

    void FixedUpdate()
   {
        if (_selecting)
        {
            AddGravity();
        }

         float acc = -(gravity + _addedGravity) * _rigidbody.mass;
         _velocity += acc * Time.deltaTime;

         transform.position += Vector3.up * (_velocity * Time.deltaTime);
         if (_minY >= transform.position.y)
         {
            transform.position = new Vector3(transform.position.x, _minY, transform.position.z);
            if (_addedGravity > 0)
               _addedGravity = 0;
            _velocity = 0;
         }

        if (_addedGravity != 0f)
         {
            _sec += Time.deltaTime;
            if (_sec >= maxSec)
            {
                _sec = 0f;
                _addedGravity = 0f;
                _velocity = 0f;
            }

        }
    }

   private void AddGravity()
   {
        // Input
        float axis = Input.GetAxis("Mouse ScrollWheel");

        if (_prevAxis * axis < 0)
        {
            _tempAddedGravity = 0f;
            _velocity = 0f;
        }

        if (axis < 0)
            _tempAddedGravity += power / _rigidbody.mass;
        else if (axis > 0)
            _tempAddedGravity -= power / _rigidbody.mass * power;

        _prevAxis = axis;

        // UI
        var color = _renderer.material.GetColor("_EmissionColor");
        if (axis > 0)
        {
            // r -> b
            color.r -= 0.01f * _rigidbody.mass;
            color.g -= 0.01f * _rigidbody.mass;
            color.b += 0.01f * _rigidbody.mass;
        }
        else if (axis < 0)
        {
            // b -> r
            color.r += 0.1f;
            color.g -= 0.1f;
            color.b -= 0.1f;
        }

        if (_tempAddedGravity == 0f)
        {
            color = _defaultColor;
        }
        _renderer.material.SetColor("_EmissionColor", color);
    }

    public void SelectObject(Material mat)
    {
        // 선택시 메터리얼을 중력 UI용 메터리얼로 변경
        _renderer.material = mat;
        _selecting = true;
        _defaultColor = mat.GetColor("_EmissionColor");
    }

    public void AdaptChanges()
    {
        _addedGravity = _tempAddedGravity;
        _tempAddedGravity = 0f;
        _renderer.material = _defaultMat;
        _selecting = false;
    }

}



