using UnityEngine;

namespace BT_Tree_Workshop
{
    public class BT_Sequence : BT_Node
    {

        public BT_Sequence(string name) : base(name)
        {
        }

        public override BT_Status Process()
        {
            // each leaf is performed one by one
            // RUNNING : if a leaf is running
            // FAILURE : if ONE leaf failed
            // SUCCESS : when the last leaf succeeds
            if (_childIdx >= _children.Count)
                return BT_Status.Success;

            BT_Status currentStatus = _children[_childIdx].Process();
            Debug.Log("Current Action : " + _children[_childIdx].Name);

            if (currentStatus == BT_Status.Success)
            {
                _childIdx++;

                if (_childIdx >= _children.Count)
                {
                    return BT_Status.Success;
                }
                else
                {
                    Debug.Log("New Action : " + _children[_childIdx].Name);
                    return BT_Status.Running;
                }
            }

            return currentStatus;

        }
    }
}
