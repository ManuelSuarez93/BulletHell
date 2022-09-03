using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } 
        [SerializeField] private Player.Player player;
        private int _score = 0;
        private int _currentLevel = 0;
        private int _currentDifficulty = 0;
        public GameObject PlayerObject => player.gameObject;
        public Transform PlayerTransform => player.transform; 
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

        public void SetScore(int newScore) => _score = newScore;
    }

}
