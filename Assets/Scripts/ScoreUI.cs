using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private SongProfiler.PlayerNumber playerNumber;
    
    private Animator _scoreAnimator;
    private Animator _comboAnimator;

    public static ScoreUI p1Score;
    public static ScoreUI p2Score;
    void Awake()
    {
        _scoreAnimator = _scoreText.GetComponent<Animator>();
        _comboAnimator = _comboText.GetComponent<Animator>();

        if (playerNumber == SongProfiler.PlayerNumber.Player1) p1Score = this;
        if (playerNumber == SongProfiler.PlayerNumber.Player2) p2Score = this;
    }
    
    public void SetScore(int score)
    {
        _scoreText.text = $"{score}";
        _scoreAnimator.SetTrigger("Update");
    }

    public void SetCombo(int combo)
    {
        _comboText.text = $"{combo}x";
        _comboAnimator.SetTrigger("Update");

    }
}
