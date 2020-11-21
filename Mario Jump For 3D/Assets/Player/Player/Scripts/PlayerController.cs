using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        // animation
        private Animator _animator;
        // 캐릭터 애니매이션 파라미터 해쉬값
        private readonly int _hashForward = Animator.StringToHash("Forward");
        private readonly int _hashBack = Animator.StringToHash("Back");
        private readonly int _hashLeft = Animator.StringToHash("Left");
        private readonly int _hashRight = Animator.StringToHash("Right");
        private readonly int _hashJump = Animator.StringToHash("Jump");
        private readonly int _hashDash = Animator.StringToHash("Dash");
        
        

       // private CharacterController _characterController;
        private Rigidbody _rigidbody;

        [Header("Moving Data")]

        [SerializeField] private float speed;

        // player State
        [SerializeField]private PlayerState _state = PlayerState.Idle;

       // rotate
        private Vector2 _mouseInputVector;
        [SerializeField] private Transform cameraBody;
        [SerializeField] private Transform cameraLookat;
        public float minY, maxY;
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;

        // Jump
        private bool _isGrounded = false;

        // Dash
        private int _idleTag;
        
        // Skill
        private bool _selectMode;
        
        public bool IsGrounded
        {
            get => _isGrounded;
            set => _isGrounded = value;
        }

        void Awake()
        {
            _animator = GetComponent<Animator>();
           // _characterController = GetComponent<CharacterController>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        void Update()
        {
            InputKey();
            Rotate();
            
        }

        private void FixedUpdate()
        {
            if (_state == PlayerState.Dead)
            {
                // Dead Animation
                return;
            }

            if (_state == PlayerState.Jump)
            {
                Jump();
                Debug.Log("JumpState");
            }
            if (_state == PlayerState.Run)
            {
                Run();
            }
            else if (_state == PlayerState.Dash)
            {
                
                return;
            }
            else if (_state == PlayerState.Fall)
            {
                Falling();
            }
            else if (_state == PlayerState.Attack)
            {
                
            }
            else if (_state == PlayerState.Idle)
            {
                _idleTag = _animator.GetCurrentAnimatorStateInfo(0).GetHashCode();
            }
        }

        void InputKey()
        {
            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                _state = PlayerState.Jump;
                Debug.Log("JUMP");
                //if(_isGrounded)
                    _animator.SetTrigger(_hashJump);
            }
            
            // Dash
            if (Input.GetButtonDown("Dash"))
            {
                _state = PlayerState.Dash;
                if (_state != PlayerState.Dash)
                {
                    _animator.SetTrigger(_hashDash);
                }
            }
            
            // Select
            if (Input.GetButton("Left Click"))
            {
                _selectMode = true;
            }
            
            // Run
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                _state = PlayerState.Run;
            }

            // Move Animation
            if (Input.GetKeyDown(KeyCode.W))
            {
                _animator.SetBool(_hashForward, true);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _animator.SetBool(_hashBack, true);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if(!Input.GetButtonDown("Vertical"))
                    _animator.SetBool(_hashLeft,true);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if(!Input.GetButtonDown("Vertical"))
                    _animator.SetBool(_hashRight,true);
            }
            
            if (Input.GetKeyUp(KeyCode.W))
            {
                _animator.SetBool(_hashForward, false);
            } 
            if (Input.GetKeyUp(KeyCode.S))
            {
                _animator.SetBool(_hashBack, false);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                _animator.SetBool(_hashLeft,false);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                _animator.SetBool(_hashRight, false);
            }
        }
        

        private void LateUpdate()
        {
            if(_animator.GetBool(_hashJump))
                _animator.SetBool(_hashJump, false);
        }

        void CheckFalling()
        {

        }
        private void Falling()
        {
         
        }    
        private void Run()
        {
            // animation selec
            //var dir = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")) * (Time.deltaTime * speed);
            var dir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (dir == Vector3.zero)
                _state = PlayerState.Idle;
            //_characterController.Move(dir);
            transform.Translate(dir * (speed * Time.deltaTime));
        }

        void Dash()
        {
            
        }

        void Jump()
        {
            Run();
        }

        private void Rotate()
        {
            var rot = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up* (rot*Time.deltaTime*sensitivityX));
            var camY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY;
            cameraBody.position = new Vector3(cameraBody.position.x, Mathf.Clamp(cameraBody.position.y - camY ,
                cameraLookat.position.y - minY, cameraLookat.position.y + maxY),cameraBody.position.z);
        }
    }
}

