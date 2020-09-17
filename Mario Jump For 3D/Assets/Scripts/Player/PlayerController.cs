using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigid;
        private Transform _transform;

        public float rotationAngle = 30.0f;
        public float movingSpeed = 1.0f;

        public float jumpForce = 1.0f;
        public float acceleraion = 1.0f;
        public float maxJumpingDistance = 10.0f;
        private float _maxJumpingHeight;
        private bool _isJumping = false;
        private bool _isFalling = false;

        
        // Start is called before the first frame update
        void Start()
        {
            _transform = GetComponent<Transform>();
            _rigid = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            Jump();
        }

        private void Move()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            
            _transform.Translate(0.0f,0.0f,v*Time.deltaTime*movingSpeed);
            _transform.Rotate(Vector3.up,h*rotationAngle * Time.deltaTime);
        }

        private void Jump()
        {
            if (_isFalling)
            {
                _rigid.AddForce(acceleraion * Vector3.down,ForceMode.Acceleration);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
            {
                _maxJumpingHeight = _transform.position.y + maxJumpingDistance;
                _isJumping = true;
                _rigid.AddForce(jumpForce * Vector3.up ,ForceMode.Impulse);
                return;
            }

            if (!_isJumping)
                return;

            if (_isJumping)
            {
                if (_transform.position.y >= _maxJumpingHeight)
                {
                    _isFalling = true;
                    return;
                }
                _rigid.AddForce(Vector3.up*acceleraion,ForceMode.Acceleration);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isJumping & other.gameObject.CompareTag("FLOOR"))
            {
                _isJumping = false;
                _isFalling = false;
            }
        }
    }
}

