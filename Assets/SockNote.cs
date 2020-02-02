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

    public GameObject sockAppearance;

    public List<NoteEffectAnimator> effects;

    [Header("Donut touch")]
    public float t;
    public float y;

    private AudioSource hitSource;
    private bool _played = false;

    public Sock sock;

    public SongProfiler.PlayerNumber owner;
    
    void Start()
    {
        hitSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        // Update Y position based on target
        t = ((float) AudioSettings.dspTime - startDsp) / (killDsp - startDsp);
        y = Mathf.Lerp(RhythmManager.Instance.spawnY,RhythmManager.Instance.killY, t);
        
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void SetSock(Sock givenSock)
    {
        sock = givenSock;
        sockAppearance.GetComponent<SpriteRenderer>().sprite = sock.sprite;

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
}
