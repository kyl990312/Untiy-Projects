using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour, PlayerInput.IPlayerMovingActions
    {
        // animation
        private Animator _animator;
        // 캐릭터 애니매이션 파라미터 해쉬값
        private readonly int _hashWalk = Animator.StringToHash("IsWalking");
        private readonly int _hashJump = Animator.StringToHash("Jump");

        private Transform _transform;

        private CharacterController _characterController;

        [Header("Moving Data")] 
        private PlayerInput _playerInputAction;

        [SerializeField] private float speed;
        private Vector2 _moveInputVector;
        private Vector2 _inputVector;
        
        // player State
       [SerializeField]private PlayerState _state = PlayerState.Idle;
       
        
        // falling
        [SerializeField] private float fallingSpeed = 1.0f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float GroundedCheckOffset = 0.166f;
        [SerializeField] private float GroundedCheckRadius = 0.17f;
        

        private const float _gravity = -9.81f;

        // rotate
        private Vector2 _mouseInputVector;
        [SerializeField] private float sensitivity;

        // jump
        private bool _jumping;
        
        public bool Jumping
        {
            set
            {
                _jumping = value;
            }
            get
            {
                return _jumping;
            }
        }
        void Awake()
        {
            _animator = GetComponent<Animator>();
            _transform = GetComponent<Transform>();
            _characterController = GetComponent<CharacterController>();
        }

        void OnEnable()
        {
            if(_playerInputAction == null) _playerInputAction = new PlayerInput();
            _playerInputAction.PlayerMoving.SetCallbacks(this);
            _playerInputAction.PlayerMoving.Enable();

        }

        private void OnDisable()
        {
            _playerInputAction.PlayerMoving.Disable();

        }

        void Update()
        {
            Rotate();
            

            if (_state == PlayerState.Dead)
            {
                // Dead Animation
                return;
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
                _animator.SetBool(_hashWalk,false);
                if (_inputVector != Vector2.zero)
                    _state = PlayerState.Run;
                
            }
        }
        

        private void LateUpdate()
        {
            if(_animator.GetBool(_hashJump))
                _animator.SetBool(_hashJump, false);
        }

        private void FixedUpdate()
        {
            // Check Falling
            if (!_jumping)
            {
                if (!Physics.CheckSphere(
                    new Vector3(transform.position.x, transform.position.y + GroundedCheckOffset, transform.position.z),
                    GroundedCheckRadius, groundLayer))
                    _state = PlayerState.Fall;
                else
                {
                    fallingSpeed = 1f;
                    if (_inputVector == Vector2.zero)
                        _state = PlayerState.Idle;
                    else
                    {
                        _state = PlayerState.Run;
                    }
                }
            }
        }

        private void Falling()
        {
            if (_jumping) return;
            // set fall animation
            if (fallingSpeed > 1f || _inputVector == Vector2.zero) 
                _animator.SetBool(_hashWalk,false);

            var dir = ((transform.forward * _inputVector.y + transform.right * _inputVector.x) * speed +
                       transform.up * fallingSpeed) * Time.deltaTime;

            fallingSpeed += Time.deltaTime * _gravity * 1.2f;
            _characterController.Move(dir);
        }    
        private void Run()
        {
            // animation select
            _animator.SetBool(_hashWalk, true);
            if (_inputVector.y == 0)
            {
                if (_inputVector.x > 0)
                {
                    // right run animation
                }
                else
                {
                    //left run animation
                }
            }

            if (_inputVector.y > 0)
            {
                _animator.SetBool(_hashWalk, true);
            }
            else if(_inputVector.y < 0)
            {
                // back run animation
                _animator.SetBool(_hashWalk, true);
            }

            // move
            if (_inputVector == Vector2.zero)
            {
                _animator.SetBool(_hashWalk, false);
                _state = PlayerState.Idle;
            }

            var dir = (transform.forward * _inputVector.y + transform.right * _inputVector.x) *
                      (Time.deltaTime * speed);
            _characterController.Move(dir);
        }

        private void Rotate()
        {
            _transform.Rotate(new Vector3(0f,_mouseInputVector.x,0f) * (Time.deltaTime*sensitivity));
        }

        // Input
        public void OnMove(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>();
            _state = PlayerState.Run;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!_jumping && _state != PlayerState.Fall)
            {
                _jumping = true;
                _animator.SetBool(_hashJump, true);
            }
        }
        
        public void OnAim(InputAction.CallbackContext context) 
        {
            _mouseInputVector = context.ReadValue<Vector2>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(transform.position.x,transform.position.y + GroundedCheckOffset,transform.position.z),GroundedCheckRadius);
        }
        
    }
}

