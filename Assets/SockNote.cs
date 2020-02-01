using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockNote : MonoBehaviour
{
    public float startDsp;
    public float endDsp;

    [Header("Donut touch")]
    public float t;
    public float y;

    void Update()
    {
        // Update Y position based on target
        t = ((float) AudioSettings.dspTime - startDsp) / (endDsp - startDsp);
        y = Mathf.Lerp(RhythmManager.Instance.spawnY,RhythmManager.Instance.targetY, t);
        
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        
        // TODO Remove, debug code
        if (AudioSettings.dspTime > endDsp)
        {
            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
        
    }
}
