// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/MasterInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MasterInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MasterInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MasterInput"",
    ""maps"": [
        {
            ""name"": ""Menu"",
            ""id"": ""9c59630d-27b4-427b-aa13-937877cc3676"",
            ""actions"": [
                {
                    ""name"": ""Pause Game"",
                    ""type"": ""Button"",
                    ""id"": ""5f85a334-f9de-43bc-afff-378ca56e371a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1fe61faf-ce7d-4f8f-bcb2-85b945a9f9bb"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Pause Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player"",
            ""id"": ""2053abdd-8dda-4bb0-8718-dca75bcaead4"",
            ""actions"": [
                {
                    ""name"": ""SkipSequence"",
                    ""type"": ""Button"",
                    ""id"": ""cc51920c-6859-40b5-b81a-0be710b96553"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""529e0d2c-bad0-4e29-ba63-87699dde49a7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RestartLevel"",
                    ""type"": ""Button"",
                    ""id"": ""3d7a6ef4-f2d2-4763-9dc4-12d9ccb33950"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5ff85c02-a797-4341-82c5-4123389ceb85"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SkipSequence"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03bb901e-923e-41ed-b31c-cd257755abef"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9698a53a-8dc2-4cb6-a2c6-1fa62b5f7eb8"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RestartLevel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_PauseGame = m_Menu.FindAction("Pause Game", throwIfNotFound: true);
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_SkipSequence = m_Player.FindAction("SkipSequence", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_RestartLevel = m_Player.FindAction("RestartLevel", throwIfNotFound: true);
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

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_PauseGame;
    public struct MenuActions
    {
        private @MasterInput m_Wrapper;
        public MenuActions(@MasterInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PauseGame => m_Wrapper.m_Menu_PauseGame;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @PauseGame.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnPauseGame;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_SkipSequence;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_RestartLevel;
    public struct PlayerActions
    {
        private @MasterInput m_Wrapper;
        public PlayerActions(@MasterInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @SkipSequence => m_Wrapper.m_Player_SkipSequence;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @RestartLevel => m_Wrapper.m_Player_RestartLevel;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @SkipSequence.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkipSequence;
                @SkipSequence.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkipSequence;
                @SkipSequence.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkipSequence;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @RestartLevel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRestartLevel;
                @RestartLevel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRestartLevel;
                @RestartLevel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRestartLevel;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SkipSequence.started += instance.OnSkipSequence;
                @SkipSequence.performed += instance.OnSkipSequence;
                @SkipSequence.canceled += instance.OnSkipSequence;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @RestartLevel.started += instance.OnRestartLevel;
                @RestartLevel.performed += instance.OnRestartLevel;
                @RestartLevel.canceled += instance.OnRestartLevel;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    public interface IMenuActions
    {
        void OnPauseGame(InputAction.CallbackContext context);
    }
    public interface IPlayerActions
    {
        void OnSkipSequence(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnRestartLevel(InputAction.CallbackContext context);
    }
}
