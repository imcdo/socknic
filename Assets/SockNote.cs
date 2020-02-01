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

    [Header("Donut touch")]
    public float t;
    public float y;

    private AudioSource hitSource;
    private bool _played = false;
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

    // Call when this Note is hit by a player
    public void Hit()
    {
        Debug.Log("boop");
        //  TODO score based on accuracy here
        Destroy(gameObject);
    }
}
