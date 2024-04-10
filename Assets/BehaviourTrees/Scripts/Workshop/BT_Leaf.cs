using System;
using UnityEngine;

namespace BT_Tree_Workshop
{
    public class BT_Leaf : BT_Node
    {
        private Func<BT_Status> _action;

        public BT_Leaf(string name, Func<BT_Status> action) : base(name)
        {
            _action = action;
        }

        public override BT_Status Process()
        {
            return _action();
        }
    }
}
