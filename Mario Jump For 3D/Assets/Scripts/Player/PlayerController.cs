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
        
        private Rigidbody _rigidbody;
        private Transform _transform;

        [Header("Move Data")] 
        public float speed = 1.0f;
        private float _speed;
        private int _backAngle = 0;

        [Header("Jump Data")] 
        public float jumpForce = 1.0f;
        
        void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
            
            // 정해진 이동 속도에 deltaTime 적용
            _speed = speed * Time.deltaTime;
        }

        void Update()
        {
            Jump();
            Move();
        }

        private void Move()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Rotation
                _backAngle += 180;
                transform.rotation = Quaternion.Euler(new Vector3(0, _backAngle, 0));
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                // Move Animation
                _animator.SetBool(_hashWalk, true);
                
                // Translate
                _transform.Translate(Vector3.forward * _speed);
                return;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
               // Move Animation
               _animator.SetBool(_hashWalk, true);
                
               // Translate
               _transform.Translate(Vector3.forward * _speed);
               return;
            }
                
            // Idle Animation
            _animator.SetBool(_hashWalk, false);
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

