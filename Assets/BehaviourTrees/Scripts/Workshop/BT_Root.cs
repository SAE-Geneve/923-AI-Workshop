using UnityEngine;

namespace BT_Tree_Workshop
{
    public class BT_Root : BT_Node
    {

        public BT_Root(string name) : base(name)
        {
        }

        public override BT_Status Process()
        {
            Debug.Log("Root processing " + _children[_childIdx].Name);
            return _children[_childIdx].Process();
        }
    }
}
