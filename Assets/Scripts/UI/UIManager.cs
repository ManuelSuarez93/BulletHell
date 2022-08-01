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
    [SerializeField] private TextMeshProUGUI _weaponName;
    int _score;

    void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 

        _score = 0;
    } 
    public void SetHealth(int h) => _lifeText.text = $"LIFE:{h}";
    public void SetWeaponName(string name) => _weaponName.text = $"WEAPON: {name}";
    public void AddToScore(int s)
    {
        _score += s;
        _scoreText.text = $"SCORE:{_score}";
    }

}
