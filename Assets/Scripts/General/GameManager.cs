using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } 
        private int _score = 0;

        public int Score => _score;

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
