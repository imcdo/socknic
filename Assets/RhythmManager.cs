    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using Note = SongProfiler.Note;

// TODO Could make this let people register to UnityEvents that return information about beats for portability but I'm lazy (~:
public class RhythmManager : MonoBehaviour
{
    private static RhythmManager _instance;
    public static RhythmManager Instance => _instance;
    
    public GameObject notePrefab;
    
    // Seconds
    public float introDelay;
    // Time between spawn and getting to the target
   
    // TODO: acount for jump
    public float dropTime  =>  spawnToTargetDistance / currentSong.approachRate;
    public float killTime  =>  spawnToKillDistance / currentSong.approachRate;
    
    public SongConfig currentSong;
    public AudioSource musicSource;

    public AudioSource hitSource;
    
    // Marker and target (Only uses Y)
    public GameObject spawn;
    public GameObject target;
    public GameObject kill;
    
    
    [Header("Donut touch")]
    // When song starts
    public bool songStarted;
    public float beatInterval;
    // Time for next beat to be SPAWNED
    public float nextBeatSpawnTime;

    public float audioStartTime;
    private float spawnToTargetDistance;
    private float spawnToKillDistance;
    public float spawnY { get; private set; }
    public float targetY { get; private set; }
    public float killY { get; private set; }

    private SongProfiler _songProfiler;
    private int _noteI;
    private Note _currNote => _songProfiler.Song[_noteI];
    void Awake()
    {
        if (_instance != null) Destroy(gameObject);
        _instance = this;
        _songProfiler = GetComponent<SongProfiler>();

    }
    
    
    void Start()
    {
        // Get info about the note spawn/target
        spawnY = spawn.transform.position.y;
        targetY = target.transform.position.y;
        killY = kill.transform.position.y;
        spawnToTargetDistance = Mathf.Abs(targetY - spawnY);
        spawnToKillDistance = Mathf.Abs(killY - spawnY);
        
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

        // Time the next beat should hit the target
        nextBeatSpawnTime = audioStartTime - dropTime + _currNote.noteTime;
        if (nextBeatSpawnTime < audioStartTime) Debug.LogWarning("first beat before song");
        // Fixes spawn to skip first beat if it's supposed to spawn before the song starts
//        while (nextBeatSpawnTime < audioStartTime)
//        {
//            nextBeatSpawnTime += beatInterval;
//        }
        
        musicSource.Play();
    }

    void Update()
    {
        
        if (songStarted)
        {
            float currentDspTime = (float) AudioSettings.dspTime;
            
            // Time to spawn a note!
            if (currentDspTime >= nextBeatSpawnTime)
            {
                GameObject noteObj = Instantiate(notePrefab, new Vector2(_currNote.xPosition,0), Quaternion.identity, spawn.transform);
                SockNote note = noteObj.GetComponent<SockNote>();
                note.startDsp = nextBeatSpawnTime;
                note.targetDsp = nextBeatSpawnTime + dropTime;
                note.killDsp = nextBeatSpawnTime + killTime;
                
                _noteI++;
                nextBeatSpawnTime = audioStartTime + _currNote.noteTime- dropTime;
            } 
        }
    }
}
