using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace BehaviourTrees
{
    public class BT_Node
    {
        public enum NodeStatus {SUCCESS, RUNNING, FAILURE}

        public NodeStatus Status { get => _status; set => _status = value;}
        private NodeStatus _status;
        
        public int Level { get => _level; set => _level = value;}
        private int _level;
        
        //public List<BT_Node> Children => _children;
        protected List<BT_Node> _children = new List<BT_Node>();
        // TODO : Make Current child an iterator (safer, cleaner)
        protected int _currentChildIdx = 0;

        public BT_Node CurrentChild => _children[_currentChildIdx];
        
        public string Name => _name;
        protected String _name;

        protected BT_Node(){ }
        public BT_Node(string name)
        {
            _name = name;
        }

        public void AddChild(BT_Node node)
        {
            node.Level = Level + 1;
            _children.Add(node);
        }
        public override string ToString()
        {
            String treeString = new String('-', 5 * _level) + _name + "\n";

            foreach (var child in _children)
            {
                treeString += child.ToString();
            }

            return treeString;

        }
        public virtual NodeStatus Process()
        {
            return _children[_currentChildIdx].Process();
        }

    }

}
