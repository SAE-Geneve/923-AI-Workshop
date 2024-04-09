using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTrees
{
    public class BT_Selector : BT_Node
    {
        public BT_Selector(string name) : base(name)
        {
        }

        public override NodeStatus Process()
        {
            NodeStatus childStatus = _children[_currentChildIdx].Process();
            
            if (childStatus == NodeStatus.FAILURE)
            {
                _currentChildIdx++;

                if (_currentChildIdx >= _children.Count)
                {
                    _currentChildIdx = 0;
                    childStatus = NodeStatus.FAILURE;
                    // End of the sequence
                }
                else
                {
                    childStatus = NodeStatus.RUNNING;
                    // Still a leaf to process
                }

                Debug.Log("New node processing : " + _children[_currentChildIdx].Name);

            }

            return childStatus;

        }

    }
}