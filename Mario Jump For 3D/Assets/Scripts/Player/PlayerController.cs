using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigid;
        private Transform _transform;

        public float rotationAngle = 30.0f;
        public float movingSpeed = 1.0f;

        public float jumpForce = 1.0f;
        public float acceleraion = 3f;
        public float maxJumpingDistance = 10.0f;
        private float _maxJumpingHeight;
        private bool _isJumping = false;
        private bool _isGrounded = false;

        // Start is called before the first frame update
        void Start()
        {
            _transform = GetComponent<Transform>();
            _rigid = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            JumpInput();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
            Fall();    
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
            if (_isJumping)
            {
                if (_transform.position.y >= _maxJumpingHeight)
                {
                    _isJumping = false;
                    return;
                }

                _rigid.velocity = (Vector3.up * acceleraion);
            }
        }

        private void Fall()
        {
            if (_isJumping)
                return;
            if (!_isGrounded)
                _rigid.velocity = (acceleraion * Vector3.down);
        }

        private void JumpInput()
        {
            if (!_isGrounded)
                return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _maxJumpingHeight = _transform.position.y + maxJumpingDistance;
                _isJumping = true;
                _rigid.AddForce(jumpForce * Vector3.up ,ForceMode.Impulse);
            }
        }
        private void OnCollisionEnter(Collision other)
        {
            if (!_isGrounded & other.gameObject.CompareTag("FLOOR"))
            {
                _isGrounded = true;
            }
        }
    }
}

