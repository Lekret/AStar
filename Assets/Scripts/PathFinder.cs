using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PathFinder : MonoBehaviour
{
    private Grid _grid;
    private BinaryHeap<Node> _openSet;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public void FindPath(Vector3 sourcePos, Vector3 targetPos, List<Node> path)
    {
        var sourceNode = _grid.WorldToNode(sourcePos);
        var targetNode = _grid.WorldToNode(targetPos);
        
        if (_openSet == null)
            _openSet = new BinaryHeap<Node>(_grid.NodeCount);
        else if (_openSet.Capacity < _grid.NodeCount)
            _openSet.Resize(_grid.NodeCount);
        _openSet.Clear();
        var openSet = _openSet;
        using var closedSetHandle = HashSetPool<Node>.Get(out var closedSet);
        using var neighboursHandle = ListPool<Node>.Get(out var neighbours);
        openSet.Add(sourceNode);

        while (openSet.Count > 0)
        {
            var currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(sourceNode, targetNode, path);
                break;
            }

            _grid.GetNeighbours(neighbours, currentNode);
            foreach (var neighbour in neighbours)
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    continue;

                var newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    private void RetracePath(Node sourceNode, Node targetNode, List<Node> path)
    {
        path.Clear();

        var currentNode = targetNode;
        while (currentNode != sourceNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
    }

    private int GetDistance(Node fromNode, Node toNode)
    {
        var distX = Mathf.Abs(fromNode.GridX - toNode.GridX);
        var distY = Mathf.Abs(fromNode.GridY - toNode.GridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}