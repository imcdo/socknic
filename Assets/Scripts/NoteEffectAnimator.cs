using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NoteEffectAnimator : MonoBehaviour
{
    private Animator _myAnimator;
    [HideInInspector]public SockNote sockNote;
    public string animationName;
    void Awake()
    {
        _myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float t = ((float) AudioSettings.dspTime - sockNote.startDsp) / (sockNote.targetDsp - sockNote.startDsp);
        
        if (t > 1) Destroy(gameObject);
        _myAnimator.Play(animationName, -1, t);
    }
}
