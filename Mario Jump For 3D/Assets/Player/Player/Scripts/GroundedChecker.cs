using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Scripts
{
    public class GroundedChecker : MonoBehaviour
    {
        private PlayerController _player;
        
        private void Awake()
        {
            _player = GetComponentInParent<PlayerController>();
        }
    
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Grounded");
        }
    }
}

