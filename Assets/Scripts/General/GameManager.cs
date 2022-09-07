using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } 
        [SerializeField] private Player.Player _player;
        [SerializeField] private Health _playerHealth;
        private int _score = 0;
        private int _currentLevel = 0;
        private int _currentDifficulty = 0;
        public GameObject PlayerObject => _player.gameObject;
        public Transform PlayerTransform => _player.transform; 
        public int Score => _score;
        public int Level => _currentLevel;
        public int Difficulty => _currentDifficulty;

        void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(this); 
            else 
                Instance = this; 

        }

        void Update()
        {
            if(_playerHealth.CurrentHealth < 0)
                StartCoroutine(RestartLevelCoRoutine());
        }
        IEnumerator RestartLevelCoRoutine()
        {  
            _player.gameObject.SetActive(false);
            if(PlayerPrefs.GetInt("Highscore",0) < _score)
                PlayerPrefs.SetInt("Highscore", _score);
            UIManager.Instance.SetGameOverScreen();
            yield return new WaitForSeconds(3f); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } 
        public void SetScore(int newScore) => _score = newScore;
    }

}
