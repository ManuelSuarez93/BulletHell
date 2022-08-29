using UnityEngine;
using UnityEngine.UI;
using TMPro;
using General;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText; 
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private Image _specialMeter;
    [SerializeField] private Image _health;

    void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this;  
    }
    public void SetHealth(float healthpct) => _health.fillAmount = healthpct;
    public void SetScoreText(int s) => _scoreText.text = $"SCORE:{GameManager.Instance.Score}";

    public void SetCooldownSpecial(float specailpct)
    {
        _specialMeter.color = Color.Lerp(Color.red, Color.green, specailpct);
        _specialMeter.fillAmount = specailpct;
    }

}
