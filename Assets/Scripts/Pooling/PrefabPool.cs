using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class PrefabPool : MonoBehaviour
    {
        [SerializeField] GameObject _prefab; 
        [SerializeField] int _maxPoolSize;
        Queue<GameObject> _queue = new Queue<GameObject>();

        void Awake()
        { 
            for(int i = 0; i <= _maxPoolSize; i++)
                InstantiatePrefabs();
        }

        void InstantiatePrefabs()
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
    }

}
