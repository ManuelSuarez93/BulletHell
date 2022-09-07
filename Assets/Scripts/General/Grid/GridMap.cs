using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{ 
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _gridPointSize; 
    [SerializeField] private GridPoint _point;
    [SerializeField] private LayerMask _layer; 
    [SerializeField] private bool _createGridAtStart = false;
    public LayerMask Layer => _layer; 
    
    private GridPoint[,] _grid;  

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
        
        _grid = new GridPoint[_gridSize.x ,_gridSize.y];
        _point.transform.localScale = new Vector3(_gridPointSize.x,4, _gridPointSize.y);

        for (int y = 0; y < _gridSize.y ; y++)
        {
            for (int x = 0; x < _gridSize.x ; x++)
            {
                var position = 
                    new Vector3(transform.position.x + (x * _gridPointSize.x), 
                                transform.position.y,
                                transform.position.z + (y * _gridPointSize.y));
  
                
                var newobj = Instantiate(_point, position, 
                            Quaternion.identity, transform);

                newobj.gameObject.name = $"GridObject[{x}][{y}]"; 
                _grid[x, y] = newobj;
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
       
        
        _grid = new GridPoint[0,0];
    }
    public GridPoint GetGridPoint(Vector2Int gridPoint)
    {
        var point = gridPoint;
        if(point.x > _grid.GetLength(0)) point.x = 0;
        if(point.y > _grid.GetLength(1)) point.y = 0;
        return _grid[gridPoint.x, gridPoint.y];
        
    }

    public void CheckIfOccupied() 
    {

    }
}
 
