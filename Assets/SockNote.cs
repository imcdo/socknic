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
    private bool _postMiss = false;
    void Start()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();

    }
    void Update()
    {
        Move();

        // Constantly updates score
        _scoreValue = 1 - Mathf.Abs((float) AudioSettings.dspTime - targetDsp) / Mathf.Abs(startDsp - killDsp);
        
        // It's past the point of no return
        if (!_postMiss && !hittable && AudioSettings.dspTime > targetDsp)
        {
            Miss();
        }
    }

    private void Move()
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

    private void HitEffect(SongProfiler.PlayerNumber hitPlayer)
    {
        PlayerMovement player = RhythmManager.Instance.GetPlayerMovement(hitPlayer);
        player.UpdateSock();
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
            PlayWrongEffect();;
    }

    // Time to Destroy this sock
    public void Miss()
    {
        _postMiss = true;
        
        // Wiggle
        PlayWrongEffect();
        
        // Pop the player sock and score
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
        
        // Destroy this note
        StartCoroutine(DestroyNote());
    }

    private void PlayWrongEffect()
    {
        _animator.SetTrigger("Miss");
        missSource.Play();
    }
}
