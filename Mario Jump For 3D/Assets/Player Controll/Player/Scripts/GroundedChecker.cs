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
            if (!_player.Jumping) return;
            // Grounded Check
            if(other.gameObject.CompareTag("FLOOR"))
            {
                _player.Jumping = false;
            } 
        }
    }
}

