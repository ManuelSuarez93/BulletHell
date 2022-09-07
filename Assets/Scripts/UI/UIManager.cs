using UnityEngine;
using UnityEngine.UI;
using TMPro;
using General;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText, _highScoreText, _gameOverScreen;
    [SerializeField] private Image _specialMeter;
    [SerializeField] private Image _health;

    void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this;  
    }
    void Start() => _highScoreText.text = $"HIGHSCORE:{PlayerPrefs.GetInt("Highscore", 0)}";
    public void SetHealth(float healthpct) => _health.fillAmount = healthpct;
    public void SetScoreText() => _scoreText.text = $"SCORE:{GameManager.Instance.Score}";

    public void SetCooldownSpecial(float specailpct)
    {
        _specialMeter.color = Color.Lerp(Color.red, Color.green, specailpct);
        _specialMeter.fillAmount = specailpct;
    }

    internal void SetGameOverScreen()
    { 
        _gameOverScreen.gameObject.SetActive(true);
    }
}
