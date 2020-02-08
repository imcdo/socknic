    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.Serialization;
    using Note = SongProfiler.Note;

// TODO Could make this let people register to UnityEvents that return information about beats for portability but I'm lazy (~:
public class RhythmManager : MonoBehaviour
{
    private static RhythmManager _instance;
    public static RhythmManager Instance => _instance;

    [SerializeField] private GameObject endScreen;
    public GameObject notePrefab;
    public GameObject approachCirclePrefab;
    public GameObject xIndicatorPrefab;
    
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

    public PlayerMovement playerOne;
    public PlayerMovement playerTwo;

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

        playerOne = PlayerMovement.Player1;
        playerTwo = PlayerMovement.Player2;
        
        // Get info about the note spawn/target
        spawnToTargetDistance = Mathf.Abs(targetY - spawnY);
        spawnToKillDistance = Mathf.Abs(killY - spawnY);
        spawnToJumpDistance = Mathf.Abs(jumpY - spawnY);
    }

    public PlayerMovement GetPlayerMovement(SongProfiler.PlayerNumber playerNumber)
    {
        if (playerNumber == SongProfiler.PlayerNumber.Player1)
        {
            return PlayerMovement.Player1;
        }

        if (playerNumber == SongProfiler.PlayerNumber.Player2)
        {
            return PlayerMovement.Player2;
        }

        return null;
    }
    
    void Start()
    {
        if (currentSong != null) _songProfiler.Parse(currentSong.songText);
        endScreen.SetActive(false);
    }
    
    public void SetSong(SongConfig songConfig)
    {
        // Get info about the song
        currentSong = songConfig;
        _songProfiler.Parse(currentSong.songText);
        _songProfiler.Song.Sort((Note a, Note b) => (a.noteTime - GetHitTime(a)).CompareTo(b.noteTime - GetHitTime(b)));
    }

    // Plays the set song
    public void StartRound()
    {
        endScreen.SetActive(false);

        SockManager.Instance.Test();
        // TODO(Ian) Reset everything so you can click this twice, like reset score.
        
        // Init song info
        beatInterval = 60 / (float) currentSong.bpm;
        musicSource.clip = currentSong.song;
        
        // Set up queues
        SockManager.Instance.InitSockManager();
        
        PlaySong();
        
    }
    
    private void PlaySong()
    {
        
        Debug.Log("Play Song");
        
        songStarted = true;
        _noteI = 0;
        
        audioStartTime = (float) AudioSettings.dspTime;
        
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
        if (note.position == SongProfiler.NotePosition.Jump)
            return spawnToJumpDistance / currentSong.approachRate;
        return spawnToTargetDistance / currentSong.approachRate;
    }

    private  void EndSong()
    {
        songStarted = false;
        endScreen.SetActive(true);
        Debug.Log("END");

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
                    new Vector2(_currNote.xPosition, (_currNote.position == SongProfiler.NotePosition.Jump) ? jumpY : targetY),
                    Quaternion.identity);
                
                GameObject xIndicatorObj = Instantiate(xIndicatorPrefab, 
                    new Vector2(_currNote.xPosition, spawnY),
                    Quaternion.identity);


                SockNote note = noteObj.GetComponent<SockNote>();
                
                approachCircleObj.GetComponent<NoteEffectAnimator>().sockNote = note;
                note.effects.Add(approachCircleObj.GetComponent<NoteEffectAnimator>());
                xIndicatorObj.GetComponent<NoteEffectAnimator>().sockNote = note;
                note.effects.Add(xIndicatorObj.GetComponent<NoteEffectAnimator>());
                
                note.startDsp = nextBeatSpawnTime;
                note.targetDsp = nextBeatSpawnTime + GetHitTime(_currNote);
                note.killDsp = nextBeatSpawnTime + killTime;
                
                // Sets note owner and position
                
                // Get the next Sock you should spawn
                note.owner = _currNote.player;
                Sock sock = SockManager.Instance.PopRhythmQueue(note.owner);
                note.SetSock(sock, _currNote.player, _currNote.position);

                _noteI++;
                if (_noteI < _songProfiler.Song.Count) nextBeatSpawnTime = audioStartTime + _currNote.noteTime- GetHitTime(_currNote);
                else
                {
                    // end song
                }
                Debug.Log(musicSource.clip.length  + " " + ((float) AudioSettings.dspTime - audioStartTime));

            } 
            if (musicSource.clip.length < currentDspTime - audioStartTime) EndSong();
        }
    }
}
