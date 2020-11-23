using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptGravity : MonoBehaviour
{
   private Rigidbody _rigidbody;

   public float addedGravity;
   private float velocity;
   private bool _collision;
   void Awake()
   {
      _rigidbody = GetComponent<Rigidbody>();
   }

   void FixedUpdate()
   {
      if (!_collision){
         float acc = -(9.8f + addedGravity) / _rigidbody.mass;
         velocity += acc;
         transform.position += Vector3.up * (velocity * Time.deltaTime);
      }
   }

   private void OnCollisionStay(Collision other)
   {
      if (other.gameObject.CompareTag("FLOOR"))
      {
         _collision = true;
      }
   }
}

