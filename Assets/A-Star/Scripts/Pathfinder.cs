using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder : MonoBehaviour
{

    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tilemap _tilemapDebug;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endObjective;

    [SerializeField] [Range(0.1f, 5f)] private float _heuristicFactor = 1f;

    [SerializeField] private TileBase _openTile;
    [SerializeField] private TileBase _closeTile;
    [SerializeField] private TileBase _pathTile;
    [SerializeField] private TileBase _floorTile;

    private List<AStarNode> _closedNodes;
    private List<AStarNode> _openNodes;
    private List<AStarNode> _pathFound;

    private AStarNode _currentNode;

    private List<Vector3> _neighbours = new List<Vector3>()
    {
        Vector3.up,
        new Vector3(1, 1, 0),
        Vector3.right,
        new Vector3(1, -1, 0),
        Vector3.down,
        new Vector3(-1, -1, 0),
        Vector3.left,
        new Vector3(-1, 1, 0)

    };

    public void FindPath()
    {

        _openNodes = new List<AStarNode>();
        _closedNodes = new List<AStarNode>();
        _pathFound = new List<AStarNode>();

        _tilemapDebug.ClearAllTiles();

        _openNodes.Add(
            new AStarNode(
                _startPosition.position,
                null,
                0,
                _heuristicFactor * Vector3.Distance(_startPosition.position, _endObjective.position)
            )
        );
        _currentNode = _openNodes[0];

        FindPathIteration();

    }

    public bool FindPathIteration()
    {

        var tilesOfTheMap = _tilemap.GetTilesBlock(_tilemap.cellBounds);

        if (_currentNode is not null && Vector3.Distance(_currentNode.Position, _endObjective.position) < 1f)
        {
            Debug.Log("Path found from start to end.");
            GetPath(_currentNode);
            return true;
        }

        if (_closedNodes.Count >= tilesOfTheMap.Length)
        {
            Debug.LogWarning("Path not found. Every tiles watched.");
            return true;
        }

        if (_openNodes.Count < 0)
        {
            Debug.LogWarning("Path not found. No more exploration nodes available.");
            return true;
        }

        _currentNode = _openNodes.OrderBy(n => n.F).FirstOrDefault();

        foreach (var neighbour in _neighbours)
        {
            // Anticipated calculations
            float possibleG = _currentNode.G + neighbour.magnitude;
            float possibleH = Vector3.Distance(_currentNode.Position + neighbour, _endObjective.position);
            float possibleF = possibleG + possibleH;

            Vector3Int position = new Vector3Int(
                Mathf.RoundToInt(_currentNode.Position.x + neighbour.x),
                Mathf.RoundToInt(_currentNode.Position.y + neighbour.y),
                Mathf.RoundToInt(_currentNode.Position.z + neighbour.z)
            );

            if (_tilemap.GetTile(position) != _floorTile)
            {
                Debug.Log("Not floor");
                continue;
            }

            if (_closedNodes.Exists(n => n.Position == position)
                ||
                _openNodes.Exists(n => n.Position == position && n.F > possibleF))
                continue;

            _openNodes.Add(new AStarNode(
                    position,
                    _currentNode,
                    possibleG,
                    _heuristicFactor * possibleH
                )
            );

        }

        _closedNodes.Add(_currentNode);
        _openNodes.Remove(_currentNode);

        GetPath(_currentNode);


        // Draw ------------------------------------------------------------------------
        _tilemapDebug.ClearAllTiles();
        foreach (var node in _openNodes)
        {
            _tilemapDebug.SetTile(
                new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y), Mathf.RoundToInt(node.Position.z)),
                _openTile
            );
        }
        foreach (var node in _closedNodes)
        {
            _tilemapDebug.SetTile(
                new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y), Mathf.RoundToInt(node.Position.z)),
                _closeTile
            );
        }
        foreach (var node in _pathFound)
        {
            _tilemapDebug.SetTile(
                new Vector3Int(Mathf.RoundToInt(node.Position.x), Mathf.RoundToInt(node.Position.y), Mathf.RoundToInt(node.Position.z)),
                _pathTile
            );
        }

        return false;

    }

    public void GetPath(AStarNode destination)
    {
        if (destination is null)
            return;

        _pathFound.Clear();
        AStarNode node = destination;

        while (node.Parent is not null)
        {
            _pathFound.Add(node);
            node = node.Parent;
        }


    }

    public void StartFinding()
    {
        StartCoroutine("FindAuto");
    }

    public void StopFinding()
    {
        StopCoroutine("FindAuto");
    }

    // Start is called before the first frame update
    IEnumerator FindAuto()
    {
        FindPath();

        while (!FindPathIteration())
        {
            yield return new WaitForSeconds(0.1f);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
