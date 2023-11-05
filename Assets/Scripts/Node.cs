using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3 Position;
    public float GCost; // Distance from starting node
    public float HCost; // Heuristic, distance from end node
    public float FCost => GCost + FCost; // GCost + HCost;

    public Node(Vector3 position, bool walkable)
    {
        Position = position;
        Walkable = walkable;
    }
}