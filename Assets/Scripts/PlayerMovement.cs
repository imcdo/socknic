using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float bpm = 40f; //RhythmManager.Instance.currentSong.bpm;
    float jumpHeight = 40f;//RhythmManager.Instance.jumpY - RhythmManager.Instance.targetY;
    public int m_PlayerNumber = 1; 
    Vector2 m_MovementInput;
    float m_movespeed = 10f;
    bool jumping;
    float gravity = 10f;
    private void Update() {
        Move();
        if(!jumping){
            Fall();
        }
    }
    private void Fall(){
         m_MovementInput.y -= gravity;
    }
    private void Move(){
        Vector2 movement = new Vector2(m_MovementInput.x, 0) * m_movespeed * Time.deltaTime;
        transform.Translate(movement);
    }
    private void OnMove(InputValue value){
        m_MovementInput = value.Get<Vector2>();
        m_MovementInput.y = 0;
    }
    private void OnJump(){
        if(!jumping){
            float timePassed = 0;
            while(timePassed <= bpm){
                m_MovementInput.y += (jumpHeight * Time.deltaTime);
                timePassed += Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.gameObject.layer == 8){
            jumping = false;
        }
    }
    private void OnHit(){
        Debug.Log("Hit");
    }
}
