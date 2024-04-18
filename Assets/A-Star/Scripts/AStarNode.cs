
using UnityEngine;

public class AStarNode
{

    private Vector3 _position;
    private AStarNode _parent;

    private float _g; // Displacement Cost
    private float _h; // Heuristic score

    public float F => _g + _h;
    public float G => _g;
    public Vector3 Position => _position;
    public AStarNode Parent => _parent;

    public AStarNode(Vector3 position, AStarNode parent, float g, float h)
    {
        _position = position;
        _parent = parent;
        _g = g;
        _h = h;
    }


}
