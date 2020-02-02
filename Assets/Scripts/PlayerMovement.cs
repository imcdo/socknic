using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
enum PlayerState : byte { Grounded, Jumping, Falling }

    bool hitting;

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
    [SerializeField] float _fallmultiplier = 1.1f;
    float _lowhitmultiplier = 10f;
    [SerializeField] int fastFall = 2;

    [SerializeField] private GameObject poof;
    private float _activeT = 0;
    public PlayerHitbox hitbox;
    private int _combo = 0;
    private int _score = 0;
    private float timeForLowHit = 0;
    private WaitForSeconds lowhitwait;
    private WaitForSeconds highhitwait;

    
    [SerializeField] private ScoreUI _scoreUi;
    
    private void Start(){
        groundY = RhythmManager.Instance.targetY + transform.position.y - hitbox.transform.position.y;
        jumpY = RhythmManager.Instance.jumpY + transform.position.y - hitbox.transform.position.y;
        
        _playerState = PlayerState.Grounded;
        xVel = 0;
    }

    private void Update() {
        animator.SetFloat("IdleSpeed", .5f * (float)((float)RhythmManager.Instance.currentSong.bpm / 60.0f));
        timeForLowHit = (float)((float)RhythmManager.Instance.currentSong.bpm / (_lowhitmultiplier * 60.0f));
        animator.SetFloat("LowHitSpeed", timeForLowHit / .217f);
        
        lowhitwait = new WaitForSeconds(timeForLowHit);
        highhitwait = new WaitForSeconds(timeForLowHit/1.7f);
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
                
                animator.SetFloat("JumpSpeed", 0.13333333f * (float)((float)RhythmManager.Instance.currentSong.bpm * _jumpBeatMultiplier / 60.0f));
                //Debug.Log(animator.GetFloat("JumpSpeed"));
                Jump();
                break;
            default:
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", false);
                break;
        }
        
        
        
    }

    private int CalculateNoteScore(float rawAccuracy)
    {
        int scoreThresh = 3;
        if (rawAccuracy < .3334) scoreThresh = 1;
        else if (rawAccuracy < .6667) scoreThresh = 2;

        return scoreThresh * _combo;
    }
    public int UpdateScore(float f)
    {
        if (f < 0)
        {
            _combo = 0;
            _scoreUi.SetCombo(_combo);
            return 0;
        }

        _combo++;
        // TODO Hi Ian, I added a multiplier to make it seem more pointier sorry if it breaks anything D:
        int noteScore = CalculateNoteScore(f);
        _score += noteScore * 1000;
        
        _scoreUi.SetCombo(_combo);
        _scoreUi.SetScore(_score);
        return noteScore / _combo;
    }
    private void Fall()
    {
        // Debug.Log("falling");
//        Debug.Log($"Fooll y= {transform.position.y} T: {_activeT}");

        _activeT += 60 * _jumpBeatMultiplier / RhythmManager.Instance.currentSong.bpm * 
                    Time.deltaTime * ((m_MovementInput.y < 0) ? fastFall : 1) * _fallmultiplier;
        if(_activeT>=1)
        {
            _startPhaseTime = (float)AudioSettings.dspTime;
            _playerState = PlayerState.Grounded;
            _activeT = 0;
            transform.position = new Vector2(transform.position.x, groundY);

            Instantiate(poof, transform.position + new Vector3(-1.0f, -.4f), Quaternion.identity).transform.localScale = new Vector3(.8f, .5f);
            Instantiate(poof, transform.position + new Vector3(1.0f,-.4f), Quaternion.identity).transform.localScale = new Vector2(-.8f, .5f);
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
            animator.SetBool("Falling", true);
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

        if (Math.Abs(Mathf.Sign(xVel) - Mathf.Sign(xMod)) > float.Epsilon && xVel + xMod < 1)
        {
            if (_playerState == PlayerState.Grounded)
            {
                if (xMod > 0)
                    Instantiate(poof, transform.position + new Vector3(-1.0f,-.4f), Quaternion.identity);
                else if (xMod < 0) 
                    Instantiate(poof, transform.position + new Vector3(1.0f,-.4f), Quaternion.identity).transform.localScale = new Vector2(-1, 1);
            }
        }
        
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
        Instantiate(poof, transform.position + new Vector3(-0.6f, -.6f), Quaternion.identity).transform.localScale = new Vector3(.4f, -.6f);
        Instantiate(poof, transform.position + new Vector3(0.6f,-.6f), Quaternion.identity).transform.localScale = new Vector2(-.4f, -.6f);        
        
        if(_playerState == PlayerState.Grounded)
        {
            GetComponent<AudioSource>().Play();
            
            _playerState = PlayerState.Jumping;
            _startPhaseTime = (float)AudioSettings.dspTime;
        }
    }

    private IEnumerator OnHit()
    {
        animator.SetBool("Hitting", true);
        GetComponent<AudioSource>().Play();
        
        hitbox.TryHit(playerNumber);
        if(_playerState == PlayerState.Grounded){
            yield return lowhitwait;
        } else{
            yield return highhitwait;
        }
        
        animator.SetBool("Hitting", false);
        Debug.Log(animator.GetBool("Hitting"));
    }


    // Called when the first sock is no longer relevant, like when it's Hit or Missed
    public void UpdateSock()
    {
        // Update your appearance by getting the next thing in your Queue
        sockAppearance.GetComponent<SpriteRenderer>().sprite = SockManager.Instance.PopPlayerSock(playerNumber).sprite;
    }
    
}
