using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Could make this let people register to UnityEvents that return information about beats for portability but I'm lazy (~:
public class RhythmManager : Singleton<RhythmManager>
{

    public GameObject notePrefab;
    
    // Seconds
    public float introDelay = 3.0f;
    public float outroDelay = 5.0f;
    // Time between spawn and getting to the target
    public float dropTime = 1.0f;
    
    public SongConfig currentSong;
    public AudioSource musicSource;
    
    // Marker and target (Only uses Y)
    public GameObject spawn;
    public GameObject target;
    
    [Header("Donut touch")]
    // When song starts
    public bool songStarted;
    public float beatInterval;
    // Time for next beat to be SPAWNED
    public float nextBeatSpawnTime;

    public float audioStartTime;
    public float spawnToTargetDistance;
    public float spawnY;
    public float targetY;

    void Start()
    {
        // Get info about the note spawn/target
        spawnY = spawn.transform.position.y;
        targetY = target.transform.position.y;
        spawnToTargetDistance = Mathf.Abs(targetY - spawnY);
    }
    
    public void SetSong(SongConfig songConfig)
    {
        // Get info about the song
        currentSong = songConfig;
    }

    // Plays the set song
    public void StartRound()
    {
        // Init song info
        beatInterval = 60 / (float) currentSong.bpm;
        musicSource.clip = currentSong.song;
        
        StartCoroutine(IntroSong());
        
    }

    IEnumerator IntroSong()
    {
        yield return new WaitForSeconds(introDelay);
        PlaySong();
    }

    private void PlaySong()
    {
        
        Debug.Log("Play Song");
        
        songStarted = true;
        
        audioStartTime = (float) AudioSettings.dspTime;

        // Time the next beat should hit the target
        nextBeatSpawnTime = audioStartTime - dropTime;

        // Fixes spawn to skip first beat if it's supposed to spawn before the song starts
        while (nextBeatSpawnTime < audioStartTime)
        {
            nextBeatSpawnTime += beatInterval;
        }
        
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
                GameObject note = Instantiate(notePrefab, spawn.transform);
                note.GetComponent<SockNote>().startDsp = nextBeatSpawnTime;
                note.GetComponent<SockNote>().endDsp = nextBeatSpawnTime + dropTime;

                nextBeatSpawnTime += beatInterval;
            }
            
        }
    }
}
