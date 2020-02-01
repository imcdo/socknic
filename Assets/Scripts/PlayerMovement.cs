using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
enum PlayerState : byte { Grounded, Jumping, Falling}

    float bpm = 40f; //RhythmManager.Instance.currentSong.bpm;
    float jumpHeight = 40f;//RhythmManager.Instance.jumpY - RhythmManager.Instance.targetY;
    public int m_PlayerNumber = 1; 
    Vector2 m_MovementInput;
    float m_movespeed = 10f;
    PlayerState _playerState;
    float _startPhaseTime;
    float gravity = 10f;

    float groundY;
    float jumpY;

    [SerializeField]AnimationCurve jumpCurve;
    [SerializeField]AnimationCurve fallCurve;

    [SerializeField] float _airControl = 0.5f;
    [SerializeField] int _jumpBeatMultiplier = 2;


    public PlayerHitbox hitbox;
    private void Start(){
        groundY = RhythmManager.Instance.targetY;
        jumpY = RhythmManager.Instance.jumpY;
        _playerState = PlayerState.Grounded;
    }


    

    private void Update() {
        switch (_playerState){
            case PlayerState.Jumping:
                Jump();
                break;
            case PlayerState.Falling:
                Fall();
                break;

        }

        if(_playerState == PlayerState.Falling){
            Fall();
        }
        Move();
        
    }
    private void Fall(){
        // Debug.Log("falling");
        float t = (float)(AudioSettings.dspTime - _startPhaseTime) * _jumpBeatMultiplier  / (RhythmManager.Instance.currentSong.bpm / 60);
        if(t>=1){
            _startPhaseTime = (float)AudioSettings.dspTime;
            _playerState = PlayerState.Grounded;
            transform.position = new Vector2(transform.position.x, groundY);
        }
        transform.position = new Vector2(transform.position.x, Mathf.Lerp(jumpY, groundY, fallCurve.Evaluate(t)));
        
    }
    private void Jump(){
        // Debug.Log("falling");
        float t = (float)(AudioSettings.dspTime - _startPhaseTime) * _jumpBeatMultiplier / (RhythmManager.Instance.currentSong.bpm  / 60);
        if(t >= 1){
            _startPhaseTime = (float)AudioSettings.dspTime;
            _playerState = PlayerState.Falling;
            Fall();
            return;
        }
        float adjT = jumpCurve.Evaluate(t);
        float newY =  Mathf.Lerp(groundY, jumpY, adjT);
        transform.position = new Vector2(transform.position.x, newY);
        //Debug.Log($"Jumping y= {transform.position.y} newY = {newY} T: {t} adjT: {adjT}");
    }
    private void Move(){
        
        
        Vector2 movement = m_MovementInput * m_movespeed * RhythmManager.Instance.deltaTime;
        if (_playerState != PlayerState.Grounded) movement *= _airControl;
        transform.Translate(movement);
    }
    private void OnMove(InputValue value){
        m_MovementInput = value.Get<Vector2>();
        m_MovementInput.y = 0;
    }
    private void OnJump(){
        //Debug.Log("jump");
        if(_playerState == PlayerState.Grounded){
            _playerState = PlayerState.Jumping;
            _startPhaseTime = (float)AudioSettings.dspTime;
        }
    }

    private void OnHit()
    {
        GetComponent<AudioSource>().Play();
        hitbox.TryHit();
    }
}
