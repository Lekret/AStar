using UnityEngine;

public class AStarNode : IBinaryHeapItem<AStarNode>
{
    public bool Walkable;
    public Vector3 Position;
    public int GridX;
    public int GridY;
    public AStarNode Parent;

    public int GCost; // Distance from starting node
    public int HCost; // Distance from end node

    public AStarNode(Vector3 position, bool walkable, int gridX, int gridY)
    {
        Position = position;
        Walkable = walkable;
        GridX = gridX;
        GridY = gridY;
    }

    public int FCost => GCost + HCost; // Total cost;

    public int HeapIndex { get; set; }

    public int CompareTo(AStarNode other)
    {
        var comparisonResult = FCost.CompareTo(other.FCost);
        if (comparisonResult == 0)
            comparisonResult = HCost.CompareTo(other.HCost);
        return -comparisonResult;
    }
}