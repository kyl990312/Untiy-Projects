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
        private Vector2 _mouseInputVector;
        [SerializeField] private Transform thirdCamFollow;
        [SerializeField] private Transform firstCamLooAt;
        [SerializeField] private Transform conversLookAt;
        public float minY, maxY;
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;

        // Jump
        private bool _isGrounded = false;

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

        public bool IsGrounded
        {
            get => _isGrounded;
            set => _isGrounded = value;
        }

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
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
            if (_state != PlayerState.Conversation)
                InputKey();            
        }

        private void FixedUpdate()
        {
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
            }


            if (Input.GetButtonUp("Right Click"))
            {
                if (_state == PlayerState.Skill)
                {
                    _animator.SetTrigger(_hashSkillEnd);
                    _selectMode = false;
                    _state = PlayerState.Idle;
                }
            }


            if (_state == PlayerState.Skill)
                return;

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
                _rigidbody.useGravity = false;
                Debug.Log("Dash");
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

                firstCamLooAt.position = new Vector3(firstCamLooAt.position.x, firstCamLooAt.position.y + camY * 0.8f, firstCamLooAt.position.z);

                thirdCamFollow.position = new Vector3(thirdCamFollow.position.x, Mathf.Clamp(thirdCamFollow.position.y - camY,
                    transform.position.y - minY , transform.position.y + maxY ), thirdCamFollow.position.z);

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


    }
}

