using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockNote : MonoBehaviour
{
    public float startDsp;
    public float targetDsp;
    public float killDsp;

    [Header("Donut touch")]
    public float t;
    public float y;

    void Update()
    {
        // Update Y position based on target
        t = ((float) AudioSettings.dspTime - startDsp) / (targetDsp - startDsp);
        y = Mathf.Lerp(RhythmManager.Instance.spawnY,RhythmManager.Instance.killY, t);
        
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        
        if (AudioSettings.dspTime >= targetDsp)
            // TODO: allow overlaping play
            RhythmManager.Instance.hitSource.Play();
        
        // TODO Remove, debug code
        if (AudioSettings.dspTime > killDsp)
        {
            Destroy(gameObject);
        }
        
    }
}
