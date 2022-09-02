using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{ 
    [SerializeField] private int _gridSizeX, _gridSizeY;
    [SerializeField] private Vector2 _gridSeparation; 
    [SerializeField] private GameObject[,] _grid; 
    [SerializeField] private GameObject _obj;
    [SerializeField] private LayerMask _layer; 
    [SerializeField] private bool _createGridAtStart = false;
    public LayerMask Layer => _layer; 



    void Awake() 
    { 
        if(_createGridAtStart)
            InitializeGrid();

    }
    [ContextMenu("Create Grid")]
    private void InitializeGrid()
    {
        if (_gridSizeX <= 0 || _gridSizeY <= 0) return;
        
        _grid = new GameObject[_gridSizeX,_gridSizeY];

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                var position = new Vector3(transform.position.x + (x + _gridSeparation.x), transform.position.y,
                            transform.position.z + (y + _gridSeparation.y));
  
                
                var newobj = Instantiate(_obj, position, 
                            Quaternion.identity, transform);

                newobj.gameObject.name = $"GridObject[{x}][{y}]"; 
                _grid[x, y] = newobj; 
            }
        } 
    }
}
 
