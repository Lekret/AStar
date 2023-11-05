using System;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    
    private Grid _grid;
    private PathFinder _pathFinder;

    private void Awake()
    {
        _grid = FindObjectOfType<Grid>();
        _pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Update()
    {
        _pathFinder.FindPath(transform.position, _target.position);
    }

    private void OnDrawGizmos()
    {
        if (_grid && _grid.Path != null)
        {
            Gizmos.color = Color.blue;
            var selfNode = _grid.WorldToNode(transform.position);
            Gizmos.DrawSphere(selfNode.Position + Vector3.up, 0.49f);

            Gizmos.color = Color.magenta;
            foreach (var pathNode in _grid.Path)
            {
                Gizmos.DrawSphere(pathNode.Position + new Vector3(0, 0.25f, 0), 0.25f);
            }
        }
    }
}