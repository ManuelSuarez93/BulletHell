using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPoint : MonoBehaviour
{ 
    [SerializeField] private Collider _collider;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private Color _emptyColor = Color.white, _fullColor = Color.red;
    List<Collider> _colliderObjects = new List<Collider>();

    public Collider Collider => _collider;
    public bool IsGridOccupied => _colliderObjects.Count > 0;  
    void Update()
    {    
        try
        { 
            foreach(var x in _colliderObjects)
            {
                if(!x.gameObject.activeSelf)    
                    _colliderObjects.Remove(x);
            }

            if(_colliderObjects.Count <= 0) 
                _mesh.material.color = _emptyColor;
        }
        catch
        {

        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(_colliderObjects.Count <= 0)
            _mesh.material.color = _fullColor;
        _colliderObjects.Add(other);
        
    }

    void OnTriggerExit(Collider other)
    {
        _colliderObjects.Remove(other);
        if(_colliderObjects.Count <= 0)
        {
            
            _mesh.material.color = _emptyColor;
        }
    }

    IEnumerator CheckIfEnabled()
    {
        while(true)
        {
           
            yield return null;
        }
    }
 
}
