namespace BT_Tree_Workshop
{
    public class BT_Selector : BT_Node
    {

        public BT_Selector(string name) : base(name)
        {
        }

        public override BT_Status Process()
        {
            BT_Status status = _children[_childIdx].Process();

            if (status == BT_Status.Failure)
            {
                _childIdx++;
                if (_childIdx >= _children.Count)
                {
                    _childIdx = 0;
                    return BT_Status.Failure;
                }
                else
                {
                    return BT_Status.Running;
                }
                    
            }

            return status;

        }
    }
}
