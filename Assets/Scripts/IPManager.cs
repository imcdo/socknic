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
        
        Debug.Log("Players: " +_inputManager.playerCount);

        _inputManager.playerPrefab = p1Prefab;
        _inputManager.JoinPlayer(controlScheme: "Player Controls2", pairWithDevices: new InputDevice[] {Gamepad.all[0], Keyboard.current });
        
        
        Debug.Log("Players: " +_inputManager.playerCount);

        _inputManager.playerPrefab = p2Prefab;
        _inputManager.JoinPlayer(controlScheme: "Player Controls2", pairWithDevices: new InputDevice[] {Gamepad.all[1], Keyboard.current,});

        Debug.Log("Players: " +_inputManager.playerCount);
        _inputManager.DisableJoining();
    }

}
