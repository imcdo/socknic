using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UpcommingSockIcon : MonoBehaviour
{
    [SerializeField] private SongProfiler.PlayerNumber _playerNumber;

    private Image _image;
    private SpriteRenderer _sr;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        if (_playerNumber == SongProfiler.PlayerNumber.Player1)
            PlayerMovement.Player1Start += P1StartIcon;
        else
            PlayerMovement.Player2Start += P2StartIcon;
    }

    private void OnDestroy()
    {
        _sr = null;
        PlayerMovement.Player1Start -= P1StartIcon;
        PlayerMovement.Player2Start -= P2StartIcon;
    }

    private void P1StartIcon()
    {
        _sr = PlayerMovement.Player1.sockAppearance.GetComponent<SpriteRenderer>();
        StartCoroutine(UpdateTick());
    }
    
    private void P2StartIcon()
    {
        _sr = PlayerMovement.Player2.sockAppearance.GetComponent<SpriteRenderer>();
        StartCoroutine(UpdateTick());
    }
    
    IEnumerator UpdateTick()
    {
        while (true)
        {
            _image.sprite = _sr.sprite;
            yield return null;
        }
    }
}
