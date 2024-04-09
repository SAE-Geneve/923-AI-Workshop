using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTrees;
using UnityEngine;

namespace BehaviourTrees
{
    public class BT_Tree : BT_Node
    {
        public BT_Tree(String prefix)
        {
            _name = prefix + "_root";
        }
        public void PrintTree()
        {
            Debug.Log(this.ToString());
        }

        // public override NodeStatus Process()
        // {
        //     if(_children.Count>=0)
        //         _currentChild = _children[0];
        //     
        //     return _currentChild.Process();
        // }
    }

}