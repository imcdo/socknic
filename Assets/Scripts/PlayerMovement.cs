using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
enum PlayerState : byte { Grounded, Jumping, Falling }

    [SerializeField] private Animator animator;

    public bool gotFirstSock = false;
    public GameObject sockAppearance;

    float bpm = 40f; //RhythmManager.Instance.currentSong.bpm;
    float jumpHeight = 40f;//RhythmManager.Instance.jumpY - RhythmManager.Instance.targetY;
    public SongProfiler.PlayerNumber playerNumber; 
    Vector2 m_MovementInput;
    PlayerState _playerState;
    float _startPhaseTime;
    float gravity = 10f;

    private float xVel;
    [SerializeField] private float maxXSpeed = 8f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float friction = .8f;
    
    float groundY;
    float jumpY;

    [SerializeField]AnimationCurve jumpCurve;
    [SerializeField]AnimationCurve fallCurve;

    [SerializeField] float _airControl = 0.5f;
    [SerializeField] int _jumpBeatMultiplier = 2;
    [SerializeField] int fastFall = 2;

    private float _activeT = 0;
    public PlayerHitbox hitbox;
    private void Start(){
        groundY = RhythmManager.Instance.targetY + transform.position.y - hitbox.transform.position.y;
        jumpY = RhythmManager.Instance.jumpY + transform.position.y - hitbox.transform.position.y;
        
        _playerState = PlayerState.Grounded;
        xVel = 0;
    }

    private void Update() {
        animator.SetFloat("IdleSpeed", .5f * (float)((float)RhythmManager.Instance.currentSong.bpm * fastFall / 60.0f));
        if(_playerState != PlayerState.Jumping && _playerState != PlayerState.Falling){
            animator.SetFloat("Horizontal", m_MovementInput.x);
        }
        
        Move();
        switch (_playerState){
            
            case PlayerState.Falling:
                animator.SetFloat("FallSpeed", 0.21666666f * (float)((float)RhythmManager.Instance.currentSong.bpm * fastFall / 60.0f));
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true);
                Fall();
                break;
            case PlayerState.Jumping:
                
                animator.SetFloat("JumpSpeed", 2*0.13333333f * (float)((float)RhythmManager.Instance.currentSong.bpm * _jumpBeatMultiplier / 60.0f));
                Debug.Log(animator.GetFloat("JumpSpeed"));
                Jump();
                break;
            default:
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", false);
                break;
        }
        
        
        
    }
    private void Fall(){
        // Debug.Log("falling");
//        Debug.Log($"Fooll y= {transform.position.y} T: {_activeT}");

        _activeT += 60 * _jumpBeatMultiplier / RhythmManager.Instance.currentSong.bpm * 
                    Time.deltaTime * ((m_MovementInput.y < 0) ? fastFall : 1);
        if(_activeT>=1)
        {
            _startPhaseTime = (float)AudioSettings.dspTime;
            _playerState = PlayerState.Grounded;
            _activeT = 0;
            transform.position = new Vector2(transform.position.x, groundY);
            return;
        }
        transform.position = new Vector2(transform.position.x, Mathf.Lerp(jumpY, groundY, fallCurve.Evaluate(_activeT)));
        
    }
    private void Jump()
    {
        animator.SetBool("Jumping", true);
        animator.SetBool("Falling", false);
        _activeT += 60 * _jumpBeatMultiplier / RhythmManager.Instance.currentSong.bpm *
                    Time.deltaTime;
        if(_activeT >= 1)
        {
            _startPhaseTime = (float)AudioSettings.dspTime;
            _playerState = PlayerState.Falling;
            animator.SetBool("Jumping", false);
            _activeT = 0;
            Fall();
            return;
        }
        float adjT = jumpCurve.Evaluate(_activeT);
        float newY =  Mathf.Lerp(groundY, jumpY, adjT);
        transform.position = new Vector2(transform.position.x, newY);
//        Debug.Log($"Jumping y= {transform.position.y} newY = {newY} T: {_activeT} adjT: {adjT}");
    }
    private void Move()
    {
        float xMod = acceleration * m_MovementInput.x;
        if (_playerState != PlayerState.Grounded) xMod *= _airControl;

        xVel = Mathf.Clamp(xVel + xMod, -maxXSpeed, maxXSpeed);
        transform.Translate(new Vector2(xVel * Time.deltaTime, 0));
        if (_playerState == PlayerState.Grounded) xVel *= (1 - friction * Time.deltaTime);
    }
    private void OnMove(InputValue value){
        m_MovementInput = value.Get<Vector2>();
        m_MovementInput.y = 0;
    }
    private void OnJump()
    {
        if(_playerState == PlayerState.Grounded)
        {
            _playerState = PlayerState.Jumping;
            _startPhaseTime = (float)AudioSettings.dspTime;
        }
    }

    private void OnHit()
    {
        GetComponent<AudioSource>().Play();
        hitbox.TryHit(playerNumber);
    }


    // Called when the first sock is no longer relevant, like when it's Hit or Missed
    public void UpdateSock()
    {
        // Update your appearance by getting the next thing in your Queue
        sockAppearance.GetComponent<SpriteRenderer>().sprite = SockManager.Instance.PopPlayerSock(playerNumber).sprite;
    }
    
}
