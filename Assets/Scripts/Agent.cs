using UnityEngine;

public class Agent : MonoBehaviour
{
    private Grid _grid;
        
    private void Awake()
    {
        _grid = FindObjectOfType<Grid>();
    }

    private void OnDrawGizmos()
    {
        if (_grid)
        {
            Gizmos.color = Color.blue;
            var node = _grid.WorldToNode(transform.position);
            Gizmos.DrawSphere(node.Position + Vector3.up, 0.49f);
        }
    }
}