using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AStarPathFinder : MonoBehaviour
{
    private AStarGrid _grid;
    private BinaryHeap<AStarNode> _openSet;

    private void Awake()
    {
        _grid = GetComponent<AStarGrid>();
    }

    public void FindPath(Vector3 sourcePos, Vector3 targetPos, List<AStarNode> path)
    {
        var sourceNode = _grid.WorldToNode(sourcePos);
        var targetNode = _grid.WorldToNode(targetPos);
        var openSet = GetOpenSet();
        var closedSet = HashSetPool<AStarNode>.Get();
        var neighbours = ListPool<AStarNode>.Get();
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
        
        HashSetPool<AStarNode>.Release(closedSet);
        ListPool<AStarNode>.Release(neighbours);
    }

    private void RetracePath(AStarNode sourceNode, AStarNode targetNode, List<AStarNode> path)
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

    private int GetDistance(AStarNode fromNode, AStarNode toNode)
    {
        var distX = Mathf.Abs(fromNode.GridX - toNode.GridX);
        var distY = Mathf.Abs(fromNode.GridY - toNode.GridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    private BinaryHeap<AStarNode> GetOpenSet()
    {
        if (_openSet == null)
            _openSet = new BinaryHeap<AStarNode>(_grid.NodeCount);
        else
            _openSet.EnsureCapacity(_grid.NodeCount);

        _openSet.Clear();
        return _openSet;
    }
}