using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour, PlayerInput.IPlayerActions
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
        private Vector2 _inputVector;
        
        // jump
        private bool _isJumping;

        // land
        private bool _isGrounded = true;
        [SerializeField]private Transform checkGroundedPos;
        [SerializeField] private LayerMask groundLayer;
        
        // falling
        [SerializeField] private float _fallingSpeed = 1.0f;
        private float gravity = -9.81f;
        
        
        void Awake()
        {
            _animator = GetComponent<Animator>();
            _transform = GetComponent<Transform>();
            _characterController = GetComponent<CharacterController>();
            
        }

        void OnEnable()
        {
            if(_playerInputAction == null) _playerInputAction = new PlayerInput();
            _playerInputAction.Player.SetCallbacks(this);
            _playerInputAction.Player.Enable();
        }

        private void OnDisable()
        {
            _playerInputAction.Player.Disable();
        }

        void Update()
        {
            Falling();
            Jump();
            Move();
        }

        private void Falling()
        {
            _isGrounded = Physics.CheckSphere(new Vector3(checkGroundedPos.position.x,checkGroundedPos.position.z,checkGroundedPos.position.y),0.1f,groundLayer);
            if (_isGrounded)
            {
                _fallingSpeed = 0f;
                _isJumping = false;
            }
            else
            {
                _fallingSpeed += Time.deltaTime * gravity;
                _characterController.Move(new Vector3(0, _fallingSpeed, 0));
            }
        }    
        private void Move()
        {
            if (_inputVector == Vector2.zero)
                _animator.SetBool(_hashWalk, false);
            var dir = (transform.forward * _inputVector.y + transform.right * _inputVector.x) *
                      (Time.deltaTime * speed);
            _characterController.Move(dir);
        }

        private void Jump()
        {
            _animator.SetBool(_hashJump,_isJumping);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>();
            _animator.SetBool(_hashWalk, true);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
                var value = context.ReadValue<float>();
                if (value == 0)
                    _isJumping = false;
                else
                    _isJumping = true;
        }
    }
}

