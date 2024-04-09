using System;
using UnityEngine;

namespace BehaviourTrees
{
    public class BT_Leaf : BT_Node
    {
        // Delegate that will be process when the leaf is processe
        public delegate NodeStatus Tick();
        private Tick _processMethod;
        
        // Leaf class specific constructors
        // protected BT_Leaf() : base() {}
        public BT_Leaf(String name, Tick processMethod) : base(name)
        {
            _processMethod = processMethod;
        }
        
        public override NodeStatus Process()
        {
            Debug.Log("Processing " + _name);
            
            if (_processMethod != null)
            {
                 return _processMethod();
            }

            return NodeStatus.FAILURE;

        }
    }

}
