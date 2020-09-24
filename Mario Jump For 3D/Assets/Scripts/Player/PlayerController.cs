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
        private Vector2 _moveInputVector;
        
        // land
        private bool _isGrounded = true;
        [SerializeField]private Transform checkGroundedPos;
        [SerializeField] private LayerMask groundLayer;
        
        // falling
        [SerializeField] private float fallingSpeed = 1.0f;
        private float gravity = -9.81f;
        
        // rotate
        private Vector2 _mouseInputVector;
        [SerializeField] private float sensitivity;
        private float _rotationAngle = 0f;


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
            Move();
            Rotate();
        }

        private void Falling()
        {
            //_isGrounded = Physics.CheckSphere(new Vector3(checkGroundedPos.position.x,checkGroundedPos.position.z,checkGroundedPos.position.y),0.1f,groundLayer);
            if (_isGrounded)
            {
                fallingSpeed = 0f;
            }
            else if(!_isGrounded)
            {
                fallingSpeed += Time.deltaTime * gravity;
                _characterController.Move(new Vector3(0, fallingSpeed, 0));
            }
        }    
        private void Move()
        {
            if (_moveInputVector == Vector2.zero)
                _animator.SetBool(_hashWalk, false);
            var dir = (transform.forward * _moveInputVector.y + transform.right * _moveInputVector.x) *
                      (Time.deltaTime * speed);
            _characterController.Move(dir);
        }

        private void Rotate()
        {
            _transform.Rotate(new Vector3(0f,_mouseInputVector.x,0f) * (Time.deltaTime*sensitivity));
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInputVector = context.ReadValue<Vector2>();
            _animator.SetBool(_hashWalk, true);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                if (_isGrounded)
                {
                    // only when player's state is grounded, play animation
                    _isGrounded = false;
                    _animator.SetBool(_hashJump, true);
                    return;
                }
            }
            _animator.SetBool(_hashJump, false);
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            _mouseInputVector = context.ReadValue<Vector2>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!_isGrounded && hit.gameObject.CompareTag("FLOOR"))
            {
                Debug.Log("Land");
                _isGrounded = true;
            }
        }
    }
}

