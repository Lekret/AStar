using System;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public void FindPath(Vector3 sourcePos, Vector3 targetPos)
    {
        var sourceNode = _grid.WorldToNode(sourcePos);
        var targetNode = _grid.WorldToNode(targetPos);
    }
}