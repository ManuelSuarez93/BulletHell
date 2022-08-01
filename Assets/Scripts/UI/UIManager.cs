using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _lifeText;
    int _score;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _score = 0;
    }
     
    void Update()
    {
        
    }

    public void SetHealth(int h)
    {
        _lifeText.text = $"LIFE:{h}";
    }
    public void SetScore(int s)
    {
        _score += s;
        _scoreText.text = $"SCORE:{_score}";
    }
}
