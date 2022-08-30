using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class PrefabPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab; 
        [SerializeField] private int _maxPoolSize;
        [SerializeField] private bool _initializeOnAwake = true;
        private Queue<GameObject> _queue = new Queue<GameObject>();
        

        private void Awake()
        {
            if (_initializeOnAwake)
                CreatePrefabs();
        }

        public void CreatePrefabs()
        {
            for (int i = 0; i <= _maxPoolSize; i++)
                InstantiatePrefabs();
        }

        private void InstantiatePrefabs()
        {
            var o = Instantiate(_prefab, transform); 
            _queue.Enqueue(o); 
            o.SetActive(false);
        }

        public GameObject GetPrefab(bool setactive)
        {
            var go = _queue.Dequeue();
            if(setactive) 
                go.SetActive(true);

            return go;
        }

        public void ReturnPrefab(GameObject go, bool Deactivate)
        {
            if(Deactivate) go.SetActive(false);
                _queue.Enqueue(go);
        }

        public void SetPrefab(GameObject newPrefab) => _prefab = newPrefab;
    }

}
