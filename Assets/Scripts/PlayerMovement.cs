using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float bpm = RhythmManager.Instance.currentSong.bpm;
    public int m_PlayerNumber = 1; 
    Vector2 m_MovementInput;
    float m_movespeed = 10f;
    bool jumping;

    private void Update() {
        Move();
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
                m_MovementInput.y += 
            }
        }
        transform.Translate(transform.up);
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
