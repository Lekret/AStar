using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3 Position;
    public int GridX;
    public int GridY;
    public Node Parent;
    
    public int GCost; // Distance from starting node
    public int HCost; // Distance from end node

    public Node(Vector3 position, bool walkable, int gridX, int gridY)
    {
        Position = position;
        Walkable = walkable;
        GridX = gridX;
        GridY = gridY;
    }

    public int FCost => GCost + HCost; // Total cost;
}