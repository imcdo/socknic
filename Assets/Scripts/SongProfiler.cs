using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SongProfiler : MonoBehaviour
{
    public struct Note
    {
        public float noteTime;
        public float xPosition;
        public bool jump;
    }

    private List<Note> _song;
    public List<Note> Song => _song;

    public void Parse(TextAsset rawSongText)
    {
        Debug.Log("Parse");
        _song = new List<Note>();
        string data = rawSongText.text.Trim();
        var lines = data.Split('\n');
        foreach (string line in lines)
        {
            var tokens = line.Trim().Split();
            
            Note note = new Note();
            if (!float.TryParse(tokens[0], out note.noteTime) 
                || !float.TryParse(tokens[1], out note.xPosition)
                || !bool.TryParse(tokens[2], out note.jump))
            {
                Debug.LogWarning("Missparse :'(");
                continue;
            }
            _song.Add(note);
        }
    }
}
