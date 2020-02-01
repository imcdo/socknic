// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player Controls1"",
            ""id"": ""692efdf5-461a-4849-b3ba-5cdeecb1659c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""643490ab-feff-468d-9a89-e219579fe148"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hit"",
                    ""type"": ""Button"",
                    ""id"": ""07000a96-6f37-44aa-8c4b-bffb15972f0b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""6a4090e6-f3cc-414c-817b-2d63456f6d98"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1c1e442d-955b-4c02-aaf9-ea72f92ebb8e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ea68162-d981-4554-8283-31fd9852f7c3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ab45aae-42ee-431e-b844-1a811b03e75f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player Controls1
        m_PlayerControls1 = asset.FindActionMap("Player Controls1", throwIfNotFound: true);
        m_PlayerControls1_Move = m_PlayerControls1.FindAction("Move", throwIfNotFound: true);
        m_PlayerControls1_Hit = m_PlayerControls1.FindAction("Hit", throwIfNotFound: true);
        m_PlayerControls1_Jump = m_PlayerControls1.FindAction("Jump", throwIfNotFound: true);
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

    // Player Controls1
    private readonly InputActionMap m_PlayerControls1;
    private IPlayerControls1Actions m_PlayerControls1ActionsCallbackInterface;
    private readonly InputAction m_PlayerControls1_Move;
    private readonly InputAction m_PlayerControls1_Hit;
    private readonly InputAction m_PlayerControls1_Jump;
    public struct PlayerControls1Actions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerControls1Actions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControls1_Move;
        public InputAction @Hit => m_Wrapper.m_PlayerControls1_Hit;
        public InputAction @Jump => m_Wrapper.m_PlayerControls1_Jump;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls1; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControls1Actions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControls1Actions instance)
        {
            if (m_Wrapper.m_PlayerControls1ActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnMove;
                @Hit.started -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnHit;
                @Hit.performed -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnHit;
                @Hit.canceled -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnHit;
                @Jump.started -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerControls1ActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_PlayerControls1ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Hit.started += instance.OnHit;
                @Hit.performed += instance.OnHit;
                @Hit.canceled += instance.OnHit;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public PlayerControls1Actions @PlayerControls1 => new PlayerControls1Actions(this);
    public interface IPlayerControls1Actions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnHit(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
