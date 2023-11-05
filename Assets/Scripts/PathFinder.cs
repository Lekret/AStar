using System;
using System.Collections.Generic;
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
        var openSet = new List<Node>();
        var closedSet = new HashSet<Node>();
        openSet.Add(sourceNode);

        while (openSet.Count > 0)
        {
            var currentNode = openSet[0];
            for (var i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || 
                    openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(sourceNode, targetNode);
                break;
            }

            var neighbours = _grid.GetNeighbours(currentNode);
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

    private void RetracePath(Node sourceNode, Node targetNode)
    {
        var path = new List<Node>();
        var currentNode = targetNode;
        while (currentNode != sourceNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        
        path.Reverse();
        _grid.Path = path;
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