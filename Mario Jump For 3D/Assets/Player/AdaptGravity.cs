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
    [SerializeField] private float power = 0.1f;

    public float tempAddedGravity
    {
      get => _tempAddedGravity;
   }
   private float _prevAxis = 0f;
   private bool _selected;
   public bool Selected
   {
      set => _selected = value;
   }
   
   private float _sec;
   public float maxSec = 15f;

   private float _velocity;
   private float _defaultY;
   private float _minY;
   void Awake()
   {
      _rigidbody = GetComponent<Rigidbody>();
      _defaultY = transform.position.y;
      Transform[] transforms = GetComponentsInChildren<Transform>();
      if (transforms[1] != null)
      {
         _minY = transforms[1].position.y;
         Debug.Log(_minY); 
         GameObject.Destroy(transforms[1].gameObject);
      }
   }

   void FixedUpdate()
   {
     
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

   public void AddGravity(float axis)
   {
        
      if (_prevAxis * axis < 0)
      {
         _tempAddedGravity = 0f;
         _velocity = 0f;
      }

      if(axis<0)
         _tempAddedGravity += _rigidbody.mass * power;
      else if (axis > 0)
         _tempAddedGravity -= _rigidbody.mass * power;

      _prevAxis = axis;
   }
   
   public void AdaptChanges()
   {
      _addedGravity = _tempAddedGravity;
      _tempAddedGravity = 0f;
   }
}



