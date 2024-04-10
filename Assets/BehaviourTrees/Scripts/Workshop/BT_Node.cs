using System.Collections.Generic;

namespace BT_Tree_Workshop
{
    public abstract class BT_Node
    {
        protected List<BT_Node> _children = new List<BT_Node>();
        protected int _childIdx = 0;
        
        private string _name;
        public string Name => _name;

        protected BT_Node(string name)
        {
            _name = name;
        }
        
        public abstract BT_Status Process();

        public void AddChild(BT_Node child)
        {
            _children.Add(child);
        }

    }

    // ReSharper disable once InconsistentNaming
    public enum BT_Status
    {
        Running,
        Success,
        Failure
    }
    
}
