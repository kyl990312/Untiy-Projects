using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Animator _animator;
        // 캐릭터 애니매이션 파라미터 해쉬값
        private readonly int _hashWalk = Animator.StringToHash("IsWalking");
        private readonly int _hashJump = Animator.StringToHash("Jump");
        
        private Transform _transform;

        private CharacterController _characterController;


        
        [Header("Moving Data")] 
        public float jumpForce = 1.0f;
        private float _deltaY = 0f;            // associate with player vertical moving
        private float _deltaX = 0f;
        private float _angleY;            // associate with player rotation
        public float inputForce = 0.1f;    // value that added at _deltaY
        public float rotatingAngle = 0.1f;    // value that added at _angleY
        private bool _moving;            // player moving state
        private int _back = 1;
        
        void Start()
        {
            _animator = GetComponent<Animator>();
            _transform = GetComponent<Transform>();
            _characterController = GetComponent<CharacterController>();
            
        }

        void Update()
        {
            Jump();
            Move();
        }

        private void Move()
        {
            // move to back
            if (Input.GetKeyDown(KeyCode.S))
            {
                // turn to back
                _angleY += 180;
                _back *= -1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                _deltaY -= inputForce * Time.deltaTime;
                _moving = true;
            }
            // move to forward
            else if (Input.GetKey(KeyCode.W))
            {
                _deltaY += inputForce* Time.deltaTime;
                _moving = true;
            }
            else
            {
                _moving = false;
                _deltaY = 0f;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                _deltaX -= inputForce * Time.deltaTime;
                _moving = true;
            }else if (Input.GetKey(KeyCode.D))
            {
                _deltaX += inputForce * Time.deltaTime;
                _moving = true;
            }else 
                _deltaX = 0;
            
            // Set Walk Animation parameter
            // if _moving is true, Walk animation will be played / if _moving is false, Idle animation will be played
            _animator.SetBool(_hashWalk, _moving);            

            // player Move
            _characterController.Move(Vector3.forward * _deltaY + Vector3.right *_deltaX);

            // player Rotate
            
        }

        private void Jump()
        {
            // Jump Animation
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _animator.SetBool(_hashJump, true);
                return;
            }
            _animator.SetBool(_hashJump, false);
        }
    }
}

