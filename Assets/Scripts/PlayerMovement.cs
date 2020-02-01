using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
enum PlayerState : byte {Jumping, Falling, Grounded}

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
    private void Start(){
        groundY = RhythmManager.Instance.targetY;
        jumpY = RhythmManager.Instance.jumpY;
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
        float t = (float)(AudioSettings.dspTime - _startPhaseTime) / (RhythmManager.Instance.currentSong.bpm / 60);
        if(t>=1){
            _playerState = PlayerState.Grounded;
            transform.position = new Vector2(transform.position.x, groundY);
        }
        transform.position = new Vector2(transform.position.x, Mathf.Lerp(jumpY, groundY, jumpCurve.Evaluate(t)));
        
    }
    private void Jump(){
        // Debug.Log("falling");
        float t = (float)(AudioSettings.dspTime - _startPhaseTime) / (RhythmManager.Instance.currentSong.bpm / 60);
        if(t >= 1){
            _playerState = PlayerState.Falling;
            Fall();
            return;
        }
        
        transform.position = new Vector2(transform.position.x, Mathf.Lerp(groundY, jumpY, jumpCurve.Evaluate(t)));
    }
    private void Move(){
        
        
        Vector2 movement = m_MovementInput * m_movespeed * RhythmManager.Instance.deltaTime;

        transform.Translate(movement);
    }
    private void OnMove(InputValue value){
        m_MovementInput = value.Get<Vector2>();
        m_MovementInput.y = 0;
    }
    private void OnJump(){
        if(_playerState != PlayerState.Jumping){
            _playerState = PlayerState.Jumping;
            _startPhaseTime = (float)AudioSettings.dspTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider){
            
        }
        Debug.Log("hit ground");

            m_MovementInput.y = 0;
    }
    private void OnHit(){
        Debug.Log("Hit");
    }
}
