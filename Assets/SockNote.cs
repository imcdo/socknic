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
    
    [SerializeField]private AudioSource hitSource;
    [SerializeField]private AudioSource missSource;

    [SerializeField] private GameObject hitParticle;

    [SerializeField] private GameObject _badHit;
    [SerializeField] private GameObject _okHit;
    [SerializeField] private GameObject _perfHit;

    
    
    private Animator _animator;
    private bool _played = false;
    [SerializeField] private AnimationCurve noteFalloffCurve;
    private SpriteRenderer[] _renderers;
    public Sock sock;

    public SongProfiler.PlayerNumber owner;
    public SongProfiler.NotePosition notePosition;
    public GameObject wings;

    public bool hittable => scoreEvaluationCurve.Evaluate(_scoreValue) > float.Epsilon;

    public AnimationCurve scoreEvaluationCurve;
    private float _scoreValue = 0;
    void Start()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();

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
        
        _scoreValue = 1 - Mathf.Abs((float) AudioSettings.dspTime - targetDsp) / Mathf.Abs(startDsp - killDsp);
        
        
        if (!hittable && AudioSettings.dspTime > targetDsp)
        {
            MissEffect();
        }
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


    private void MissEffect()
    {
        _animator.SetTrigger("Miss");
        missSource.Play();
    }

    private void HitEffect(SongProfiler.PlayerNumber hitPlayer)
    {
        PlayerMovement player = RhythmManager.Instance.GetPlayerMovement(hitPlayer);
        player.UpdateSock();
        Debug.Log(_scoreValue);
        int val = player.UpdateScore(scoreEvaluationCurve.Evaluate( _scoreValue));
        Instantiate((val == 1) ? _badHit : (val == 2) ? _okHit : _perfHit, transform.position, Quaternion.identity);

        Instantiate(hitParticle, transform.position, Quaternion.identity);
        //  TODO score based on accuracy here
        StartCoroutine(DestroyNote());
    }

    IEnumerator DestroyNote()
    {
        float timeToDestroy = .1f;
        float clock = 0;
        
        Vector3 start = new Vector3(1, 1, 1);
        Vector3 end = new Vector3(1.3f, 1.3f, 1);
        while (timeToDestroy > clock)
        {
            transform.localScale = Vector3.Lerp(start, end, clock / timeToDestroy);
            yield return null;
            clock += Time.deltaTime;
        }
        Destroy(gameObject);
    }
    
    // Call when this Note is hit by a player
    public void Hit(SongProfiler.PlayerNumber hitPlayer)
    {
        if (hitPlayer == owner && hittable)
        {
            HitEffect(hitPlayer);
        }
        else
            MissEffect();;
    }

    public void Miss()
    {
        PlayerMovement player = RhythmManager.Instance.GetPlayerMovement(owner);
        player.UpdateSock();
        player.UpdateScore(-1);
        
        
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

//    private void SetHittable(bool canHit)
//    {
//        hittable = canHit;
//
//        if (hittable)
//        {
//            // Make big
//            transform.localScale *= embiggenScale;
//        }
//        else
//        {
//            // Return to size
//            transform.localScale /= embiggenScale;
//        }
//    }
//
//    // TODO FIX
//    private void OnCollisionEnter2D(Collision2D other)
//    {
//        if (notePosition == SongProfiler.NotePosition.Ground)
//        {
//            if (other.collider.gameObject.GetComponent<LowCollider>() != null)
//            {
//                SetHittable(true);
//            }
//        }
//        
//        
//        if (notePosition == SongProfiler.NotePosition.Jump)
//        {
//            if (other.collider.gameObject.GetComponent<HighCollider>() != null)
//            {
//                SetHittable(true);
//            }
//        }
//    }
//    
//    private void OnCollisionExit2D(Collision2D other)
//    {
//        if (notePosition == SongProfiler.NotePosition.Ground)
//        {
//            if (other.collider.gameObject.GetComponent<LowCollider>() != null)
//            {
//                SetHittable(false);
//            }
//        }
//        
//        
//        if (notePosition == SongProfiler.NotePosition.Jump)
//        {
//            if (other.collider.gameObject.GetComponent<HighCollider>() != null)
//            {
//                SetHittable(false);
//            }
//        }
//    }
}
