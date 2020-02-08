using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInputManager))]
public class IPManager : MonoBehaviour
{
    private PlayerInputManager _inputManager;
    [SerializeField] private GameObject p1Prefab;
    [SerializeField] private GameObject p2Prefab;

    private void Start()
    {
        _inputManager = GetComponent<PlayerInputManager>();
        
        InputDevice[] p1Input;
        InputDevice[] p2Input;
        if (Gamepad.all.Count < 2)
        {;
            p1Input = new[] {Keyboard.current};
            p2Input = new[] {Keyboard.current};
        }
        else
        {
            p1Input = new InputDevice[] {Gamepad.all[0], Keyboard.current};
            p2Input = new InputDevice[] {Gamepad.all[1], Keyboard.current};
        }
        
        _inputManager.playerPrefab = p1Prefab;
        _inputManager.JoinPlayer(controlScheme: "Player Controls2", pairWithDevices: p1Input);
        
        _inputManager.playerPrefab = p2Prefab;
        _inputManager.JoinPlayer(controlScheme: "Player Controls2", pairWithDevices: p2Input);

        _inputManager.DisableJoining();
    }

}
