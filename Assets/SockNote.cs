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

        if (AudioSettings.dspTime >= targetDsp && !_played)
        {
            hitSource.Play();
            _played = true;
            GetComponent<SpriteRenderer>().color = Color.red;
        }
            


        // TODO Remove, debug code
        if (AudioSettings.dspTime >= killDsp)
        {
            Destroy(gameObject);
        }
    }
}
