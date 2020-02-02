using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SockNote : MonoBehaviour
{
    public float startDsp;
    public float targetDsp;
    public float killDsp;

    public float embiggenScale = 1.2f;
    
    public GameObject sockAppearance;

    public List<NoteEffectAnimator> effects;
    

    [Header("Donut touch")]
    public float t;
    public float y;

    private AudioSource hitSource;
    private bool _played = false;
    [SerializeField] private AnimationCurve noteFalloffCurve;
    private SpriteRenderer[] _renderers;
    public Sock sock;

    public SongProfiler.PlayerNumber owner;
    public SongProfiler.NotePosition notePosition;
    public GameObject wings;

    public bool hittable = false;
    
    void Start()
    {
        hitSource = GetComponent<AudioSource>();
        _renderers = GetComponentsInChildren<SpriteRenderer>();

    }
    void Update()
    {
        // Update Y position based on target
        t = ((float) AudioSettings.dspTime - startDsp) / (killDsp - startDsp);
        y = Mathf.Lerp(RhythmManager.Instance.spawnY,RhythmManager.Instance.killY, t);

        float tDeath = Mathf.Max(((float) AudioSettings.dspTime - targetDsp) / (killDsp - targetDsp), 0);
        foreach (var sr in _renderers)
            sr.color -= new Color(0, 0, 0, 1.0f - noteFalloffCurve.Evaluate(tDeath));

        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

       

    public void SetSock(Sock givenSock, SongProfiler.PlayerNumber playerNumber, SongProfiler.NotePosition notePos)
    {
        sock = givenSock;
        sockAppearance.GetComponent<SpriteRenderer>().sprite = sock.sprite;
        
        owner = playerNumber;
        notePosition = notePos;

        if (notePosition == SongProfiler.NotePosition.Jump)
        {
            wings.GetComponent<SpriteRenderer>().enabled = true;
        } else if (notePosition == SongProfiler.NotePosition.Ground)
        {
            wings.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Call when this Note is hit by a player
    public void Hit(SongProfiler.PlayerNumber hitPlayer)
    {
        if (hitPlayer == owner)
        {
            Debug.Log("boop");
            RhythmManager.Instance.GetPlayerMovement(hitPlayer).UpdateSock();
            //  TODO score based on accuracy here
            Destroy(gameObject);
        }
    }

    public void Miss()
    {
        RhythmManager.Instance.GetPlayerMovement(owner).UpdateSock();
        
        //  TODO score based on accuracy here
        
        // Cleanup any effects still in play
        foreach (NoteEffectAnimator effect in effects)
        {
            if (effect != null)
            {
                Destroy(effect.gameObject);
            }
        }
        Destroy(gameObject);
    }

    private void SetHittable(bool canHit)
    {
        hittable = canHit;

        if (hittable)
        {
            // Make big
            transform.localScale *= embiggenScale;
        }
        else
        {
            // Return to size
            transform.localScale /= embiggenScale;
        }
    }

    // TODO FIX
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (notePosition == SongProfiler.NotePosition.Ground)
        {
            if (other.collider.gameObject.GetComponent<LowCollider>() != null)
            {
                SetHittable(true);
            }
        }
        
        
        if (notePosition == SongProfiler.NotePosition.Jump)
        {
            if (other.collider.gameObject.GetComponent<HighCollider>() != null)
            {
                SetHittable(true);
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (notePosition == SongProfiler.NotePosition.Ground)
        {
            if (other.collider.gameObject.GetComponent<LowCollider>() != null)
            {
                SetHittable(false);
            }
        }
        
        
        if (notePosition == SongProfiler.NotePosition.Jump)
        {
            if (other.collider.gameObject.GetComponent<HighCollider>() != null)
            {
                SetHittable(false);
            }
        }
    }
}
