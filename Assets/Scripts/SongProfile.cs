using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SongProfile : MonoBehaviour
{
    public struct Note
    {
        public float songTime;
        public float xPosition;
        public bool jump;
    }

    public TextAsset rawSongText;
    
    
    private List<Note> _song;
    public List<Note> Song => _song;

    private void Awake()
    {
        Parse();
    }

    private void Parse()
    {
        _song = new List<Note>();
        string data = rawSongText.text.Trim();
        var lines = data.Split('\n');
        foreach (string line in lines)
        {
            var tokens = line.Trim().Split();
            
            Note note = new Note();
            if (!float.TryParse(tokens[0], out note.songTime) 
                || !float.TryParse(tokens[1], out note.xPosition)
                || !bool.TryParse(tokens[2], out note.jump))
            {
                Debug.LogWarning("Missparse :'(");
                continue;
            }

            _song.Append(note);
        }
    }
}
