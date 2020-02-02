using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillNotes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        SockNote note = other.GetComponent<SockNote>();
        if (note != null)
        {
            note.Miss();
        }
    }
}
