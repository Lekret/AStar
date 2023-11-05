using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    private Grid _grid;
    private PathFinder _pathFinder;
    private readonly List<Node> _path = new();

    private void Awake()
    {
        _grid = FindObjectOfType<Grid>();
        _pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Update()
    {
        _pathFinder.FindPath(transform.position, _target.position, _path);
    }

    private void OnDrawGizmos()
    {
        if (_grid)
        {
            Gizmos.color = Color.blue;
            var selfNode = _grid.WorldToNode(transform.position);
            Gizmos.DrawSphere(selfNode.Position + Vector3.up, 0.49f);
        }
        
        Gizmos.color = Color.magenta;
        foreach (var pathNode in _path)
        {
            Gizmos.DrawSphere(pathNode.Position + new Vector3(0, 0.25f, 0), 0.25f);
        }
    }
}