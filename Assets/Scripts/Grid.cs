using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2 _gridWorldSize;
    [SerializeField] private float _nodeSize;
    [SerializeField] private LayerMask _nonWalkableMask;

    private Node[,] _grid;
    private int _gridSizeX;
    private int _gridSizeY;

    private void Awake()
    {
        Generate();
    }

    public void Generate()
    {
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeSize);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeSize);
        FillGrid();
    }

    private void FillGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        var bottomLeft = transform.position
                         - Vector3.right * _gridWorldSize.x / 2f
                         - Vector3.forward * _gridWorldSize.y / 2f;

        var nodeHalfSize = _nodeSize / 2f;
        for (var x = 0; x < _gridSizeX; x++)
        for (var y = 0; y < _gridSizeY; y++)
        {
            var worldPoint = bottomLeft
                             + Vector3.right * (x * _nodeSize + nodeHalfSize)
                             + Vector3.forward * (y * _nodeSize + nodeHalfSize);

            var walkable = !Physics.CheckSphere(worldPoint, nodeHalfSize, _nonWalkableMask.value);
            _grid[x, y] = new Node(worldPoint, walkable);
        }
    }

    public Node WorldToNode(Vector3 worldPosition)
    {
        var percentX = (worldPosition.x + _gridWorldSize.x / 2f) / _gridWorldSize.x;
        var percentY = (worldPosition.z + _gridWorldSize.y / 2f) / _gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        var x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        var y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
        return _grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1f, _gridWorldSize.y));

        if (_grid != null)
        {
            foreach (var node in _grid)
            {
                Gizmos.color = node.Walkable ? Color.green : Color.red;
                Gizmos.DrawCube(node.Position, Vector3.one * (_nodeSize - 0.01f));
            }
        }
    }
}