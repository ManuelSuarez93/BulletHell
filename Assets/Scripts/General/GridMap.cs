using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{ 
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _gridPointSize; 
    [SerializeField] private GameObject _obj;
    [SerializeField] private LayerMask _layer; 
    [SerializeField] private bool _createGridAtStart = false;
    public LayerMask Layer => _layer; 
    
    private Collider[,] _grid; 

    
    void Awake() 
    { 
        if(_createGridAtStart)
            InitializeGrid();

    }
    [ContextMenu("Create Grid")]
    private void InitializeGrid()
    {   
        DeleteGrid();
        if (_gridSize.x <= 0 || _gridSize.y <= 0) return;
        
        _grid = new Collider[_gridSize.x ,_gridSize.y];
        _obj.transform.localScale = new Vector3(_gridPointSize.x,1, _gridPointSize.y);

        for (int y = 0; y < _gridSize.y ; y++)
        {
            for (int x = 0; x < _gridSize.x ; x++)
            {
                var position = 
                    new Vector3(transform.position.x + (x * _gridPointSize.x), 
                                transform.position.y,
                                transform.position.z + (y * _gridPointSize.y));
  
                
                var newobj = Instantiate(_obj, position, 
                            Quaternion.identity, transform);

                newobj.gameObject.name = $"GridObject[{x}][{y}]"; 
                _grid[x, y] = newobj.GetComponent<Collider>(); 
            }
        } 
    }
     [ContextMenu("Delete Grid")]
    private void DeleteGrid()
    {
        if(_grid != null && _grid.Length > 0)
        {
            foreach(var gridPoint in _grid)
                if(gridPoint)
                    DestroyImmediate(gridPoint.gameObject);
        }
       
        
        _grid = new Collider[0,0];
    }
    public Collider GetGridPoint(Vector2Int gridPoint) => _grid[gridPoint.x, gridPoint.y];

}
 
