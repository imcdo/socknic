﻿    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.Serialization;
    using Note = SongProfiler.Note;

// TODO Could make this let people register to UnityEvents that return information about beats for portability but I'm lazy (~:
public class RhythmManager : MonoBehaviour
{
    private static RhythmManager _instance;
    public static RhythmManager Instance => _instance;
    
    public GameObject notePrefab;
    public GameObject approachCirclePrefab;
    public GameObject xIndicatorPrefab;
    
    private float _lastTime;
    public float deltaTime => (float)AudioSettings.dspTime - _lastTime;
    // Seconds
    public float introDelay;
    // Time between spawn and getting to the target
   
    // TODO: acount for jump
    public float killTime  =>  spawnToKillDistance / currentSong.approachRate;
    
    public SongConfig currentSong;
    public AudioSource musicSource;

    // Marker and target (Only uses Y)
    public GameObject spawn;
    public GameObject target;
    public GameObject kill;
    public GameObject jump;
    
    
    [Header("Donut touch")]
    // When song starts
    public bool songStarted;
    public float beatInterval;
    // Time for next beat to be SPAWNED
    public float nextBeatSpawnTime;

    public float audioStartTime;
    private float spawnToTargetDistance;
    private float spawnToKillDistance;
    private float spawnToJumpDistance;
    public float spawnY { get; private set; }
    public float targetY { get; private set; }
    public float killY { get; private set; }
    public float jumpY { get; private set; }

    private SongProfiler _songProfiler;
    private int _noteI;
        private Note _currNote => _songProfiler.Song[_noteI];
    void Awake()
    {
        if (_instance != null) Destroy(gameObject);
        _instance = this;
        _songProfiler = GetComponent<SongProfiler>();
        
        spawnY = spawn.transform.position.y;
        targetY = target.transform.position.y;
        killY = kill.transform.position.y;
        jumpY = jump.transform.position.y;

        // Get info about the note spawn/target
        spawnToTargetDistance = Mathf.Abs(targetY - spawnY);
        spawnToKillDistance = Mathf.Abs(killY - spawnY);
        spawnToJumpDistance = Mathf.Abs(jumpY - spawnY);
    }
    
    
    void Start()
    {
        if (currentSong != null) _songProfiler.Parse(currentSong.songText);
    }
    
    public void SetSong(SongConfig songConfig)
    {
        // Get info about the song
        currentSong = songConfig;
        _songProfiler.Parse(currentSong.songText);
    }

    // Plays the set song
    public void StartRound()
    {
        // Init song info
        beatInterval = 60 / (float) currentSong.bpm;
        musicSource.clip = currentSong.song;
        
        PlaySong();
        
    }
    
    private void PlaySong()
    {
        
        Debug.Log("Play Song");
        
        songStarted = true;
        _noteI = 0;
        
        audioStartTime = (float) AudioSettings.dspTime;
        
        Debug.Log(_songProfiler.Song.Count);
        
        // Time the next beat should hit the target
        nextBeatSpawnTime = audioStartTime - GetHitTime(_currNote) + _currNote.noteTime;
        if (nextBeatSpawnTime < audioStartTime) Debug.LogWarning("first beat before song");
        // Fixes spawn to skip first beat if it's supposed to spawn before the song starts
//        while (nextBeatSpawnTime < audioStartTime)
//        {
//            nextBeatSpawnTime += beatInterval;
//        }
        
        musicSource.Play();
    }

    private float GetHitTime(Note note)
    {
        if (note.jump == SongProfiler.NotePosition.Jump)
            return spawnToJumpDistance / currentSong.approachRate;
        return spawnToTargetDistance / currentSong.approachRate;
    }
    
    void Update()
    {
        
        if (songStarted)
        {
            float currentDspTime = (float) AudioSettings.dspTime;
            
            // Time to spawn a note!
            while (currentDspTime >= nextBeatSpawnTime && _noteI < _songProfiler.Song.Count)
            {
                GameObject noteObj = Instantiate(notePrefab, new Vector2(_currNote.xPosition,spawnY), Quaternion.identity, spawn.transform);
                GameObject approachCircleObj = Instantiate(approachCirclePrefab, 
                    new Vector2(_currNote.xPosition, (_currNote.jump == SongProfiler.NotePosition.Jump) ? jumpY : targetY),
                    Quaternion.identity);
                
                GameObject xIndicatorObj = Instantiate(xIndicatorPrefab, 
                    new Vector2(_currNote.xPosition, spawnY),
                    Quaternion.identity);


                SockNote note = noteObj.GetComponent<SockNote>();
                
                approachCircleObj.GetComponent<NoteEffectAnimator>().sockNote = note;
                xIndicatorObj.GetComponent<NoteEffectAnimator>().sockNote = note;
                
                note.startDsp = nextBeatSpawnTime;
                note.targetDsp = nextBeatSpawnTime + GetHitTime(_currNote);
                note.killDsp = nextBeatSpawnTime + killTime;
                
                // Get the right socks to show and sets owner
                Sock sock = SockManager.Instance.GetRandomReadySock();
                note.SetSock(sock);
                note.owner = _currNote.player;

                _noteI++;
                if (_noteI < _songProfiler.Song.Count) nextBeatSpawnTime = audioStartTime + _currNote.noteTime- GetHitTime(_currNote);
                else
                {
                    // end song
                }
            } 
        }

        _lastTime = (float)AudioSettings.dspTime;
    }
}
