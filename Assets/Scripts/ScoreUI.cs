using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _comboText;

    private Animator _scoreAnimator;
    private Animator _comboAnimator;
    void Start()
    {
        _scoreAnimator = _scoreText.GetComponent<Animator>();
        _comboAnimator = _comboText.GetComponent<Animator>();
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
