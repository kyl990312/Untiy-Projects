using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
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
        private readonly int _hashSkillStart = Animator.StringToHash("SkillStart");
        private readonly int _hashSkillEnd = Animator.StringToHash("SkillEnd");
        private readonly int _hashConversationStart = Animator.StringToHash("ConversationStart");
        private readonly int _hashConversationEnd = Animator.StringToHash("ConversationEnd");
        
        

        private CharacterController _characterController;
        private Rigidbody _rigidbody;

        [Header("Moving Data")]

        [SerializeField] private float speed;

        // player State
        [SerializeField]private PlayerState _state = PlayerState.Idle;

       // rotate
        [SerializeField] private Transform thirdCamFollow;
        [SerializeField] private Transform firstCamLooAt;
        [SerializeField] private Transform conversLookAt;
        public float minY, maxY;
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;

        private Transform _default3rdCamFollow;
        private Transform _default1stCamLookAt;

        // Jump
        private bool _isGrounded = true;
        [SerializeField] private float mass;
        private float _gravity = 9.8f;
        private float _velocity;
        
        // Dash
        
        // Skill
        private bool _selectMode;
        
        // Conversation
        private Transform _npcTransform;
        public Transform npcTransform
        {
            set
            {
                _npcTransform = value;
            }
        }

        public bool SelectMode
        {
            get
            {
                return _selectMode;
                
            }
            set
            {
                _selectMode = SelectMode;
            }
        }

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            _rigidbody = GetComponent<Rigidbody>();
            _default1stCamLookAt = firstCamLooAt;
            _default3rdCamFollow = thirdCamFollow;
        }

        void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        void Update()
        {
            if (_state != PlayerState.Conversation)
                InputKey();            
        }

        private void FixedUpdate()
        {
            // Check Grounded
            CheckGrounded();


            if(_state != PlayerState.Conversation)
                Rotate();

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
                Dash();
            }
            else if (_state == PlayerState.Fall)
            {
                Falling();
            }
            else if (_state == PlayerState.Skill)
            {

            }
            else if (_state == PlayerState.Idle)
            {

            }
            else if (_state == PlayerState.Conversation) { 
                 // 대화가 끝났는지 상태를 받는다.

                 //if(대화가 끝났다면){
                 //   _animator.SetTrigger(_hashConversationEnd);
                 //   _state = PlayerState.Idle;
                 //}
            }
            
            
            if (!_isGrounded && _state != PlayerState.Jump)
            {
                var acc = -_gravity * mass;
                _velocity += acc * Time.deltaTime;
                transform.position += Vector3.up * (_velocity * Time.deltaTime);
            } else
            {
                _velocity = 0f;
            }
        }

        void InputKey()
        {
            // Select
            if (Input.GetButtonDown("Right Click"))
            {
                _animator.SetTrigger(_hashSkillStart);
                _selectMode = true;
                _state = PlayerState.Skill;
                thirdCamFollow = _default3rdCamFollow;
            }


            if (Input.GetButtonUp("Right Click") && _state == PlayerState.Skill)
            {
                _animator.SetTrigger(_hashSkillEnd);
                _selectMode = false;
                _state = PlayerState.Idle;
                firstCamLooAt = _default1stCamLookAt;
            }


            if (_state == PlayerState.Skill)
                return;

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                if (_isGrounded)
                {
                    _state = PlayerState.Jump;
                    _isGrounded = false;
                    _animator.SetTrigger(_hashJump);
                }
            }
            
            // Dash
            if (Input.GetButtonDown("Dash"))
            {
                _state = PlayerState.Dash;
                _rigidbody.useGravity = false;
                _animator.SetTrigger(_hashDash);
                if (_state != PlayerState.Dash)
                {
                    
                }
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

        void CheckFalling()
        {

        }
        private void Falling()
        {
         
        }    
        private void Run()
        {
            // animation selec
            var dir = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")) * (Time.deltaTime * speed);
            if (dir == Vector3.zero)
                _state = PlayerState.Idle;
            _characterController.Move(dir);
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
            if (_state == PlayerState.Skill)
            {
                firstCamLooAt.position = new Vector3(firstCamLooAt.position.x, firstCamLooAt.position.y + camY * 0.8f, firstCamLooAt.position.z);
            }
            else
            {
                thirdCamFollow.position = new Vector3(thirdCamFollow.position.x, Mathf.Clamp(thirdCamFollow.position.y - camY,
                    transform.position.y - minY, transform.position.y + maxY), thirdCamFollow.position.z);
            }
        }

        public void ConversationStart()
        {
            _animator.SetTrigger(_hashConversationStart);
            _state = PlayerState.Conversation;
            conversLookAt.position = _npcTransform.position;
            conversLookAt.rotation = _npcTransform.rotation;                        
        }

        public void ConversationEnd()
        {
            _animator.SetTrigger(_hashConversationEnd);
            _state = PlayerState.Idle;
        }

        private void OnCollisionStay(Collision other)
        {
            // gravity가 적용되어있는 물체 위에 있을경우
            if (other.gameObject.CompareTag("Selectable"))
            {
                transform.parent = other.transform;
                _isGrounded = true;

            }
        }

        private void OnCollisionExit(Collision other)
        {
            // gravity가 적용되는 물체 위에서 내려온 경우
            if (other.gameObject.CompareTag("Selectable"))
            {
                transform.parent = null;
            }
        }

        void CheckGrounded()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
            {
                if (hit.transform.gameObject.CompareTag("FLOOR"))
                {
                    _isGrounded = true;
                    return;
                }
            }

            _isGrounded = false;
        }
    }
}

