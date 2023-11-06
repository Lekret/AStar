using System.Collections.Generic;
using UnityEngine;

public class AStarSeeker : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    private AStarGrid _grid;
    private AStarPathFinder _pathFinder;
    private readonly List<AStarNode> _path = new();

    private void Awake()
    {
        _grid = FindObjectOfType<AStarGrid>();
        _pathFinder = FindObjectOfType<AStarPathFinder>();
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