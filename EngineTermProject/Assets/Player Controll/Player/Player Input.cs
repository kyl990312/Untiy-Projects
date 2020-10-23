// GENERATED AUTOMATICALLY FROM 'Assets/Player Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Input"",
    ""maps"": [
        {
            ""name"": ""Player Moving"",
            ""id"": ""0bbef7d8-02e3-4148-b718-0dcb2654f089"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b0b553e7-7888-4529-8cdf-e756c5f1973a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ab33ba56-d433-4a55-80a2-b95577c0b656"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""jump"",
                    ""type"": ""Button"",
                    ""id"": ""b4ea596e-5512-47ee-b631-0d8b6155ce10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""27ccdbbf-7ee9-4e61-bcad-b9f158ac8bde"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2164d325-1e0f-430a-b2c9-726913711c11"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bfee44d1-6241-4bf8-8098-f80d37a9254c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ff6a1e32-1508-4554-bd57-8366cf15a6de"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""247b02a1-6094-4d41-a82b-1d7a6955bbf4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3905c286-67de-4ff7-a2f0-59f3eface696"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""199cc9ec-f29d-470c-bbf6-37be5970712e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player Moving
        m_PlayerMoving = asset.FindActionMap("Player Moving", throwIfNotFound: true);
        m_PlayerMoving_Move = m_PlayerMoving.FindAction("Move", throwIfNotFound: true);
        m_PlayerMoving_Aim = m_PlayerMoving.FindAction("Aim", throwIfNotFound: true);
        m_PlayerMoving_jump = m_PlayerMoving.FindAction("jump", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player Moving
    private readonly InputActionMap m_PlayerMoving;
    private IPlayerMovingActions m_PlayerMovingActionsCallbackInterface;
    private readonly InputAction m_PlayerMoving_Move;
    private readonly InputAction m_PlayerMoving_Aim;
    private readonly InputAction m_PlayerMoving_jump;
    public struct PlayerMovingActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerMovingActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerMoving_Move;
        public InputAction @Aim => m_Wrapper.m_PlayerMoving_Aim;
        public InputAction @jump => m_Wrapper.m_PlayerMoving_jump;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMoving; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovingActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovingActions instance)
        {
            if (m_Wrapper.m_PlayerMovingActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnMove;
                @Aim.started -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnAim;
                @jump.started -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnJump;
                @jump.performed -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnJump;
                @jump.canceled -= m_Wrapper.m_PlayerMovingActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_PlayerMovingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @jump.started += instance.OnJump;
                @jump.performed += instance.OnJump;
                @jump.canceled += instance.OnJump;
            }
        }
    }
    public PlayerMovingActions @PlayerMoving => new PlayerMovingActions(this);
    public interface IPlayerMovingActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
