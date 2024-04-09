using System.Collections;
using System.Collections.Generic;
using BehaviourTrees;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Robber_0 : MonoBehaviour
{
    [SerializeField] private float _distanceSuccess = 5;
    
    [SerializeField] private Transform _vanTransform;
    [SerializeField] private StealableItem _diamond;
    [SerializeField] private StealableItem _sunflowers;
    [SerializeField] private OpenableDoor _frontDoor;
    [SerializeField] private OpenableDoor _backDoor;

    [SerializeField] [Range(0, 1000)] private int _currentMoney;
    [SerializeField] private int _moneyAchievment = 750;
    private StealableItem _lastStolenItem;

    private NavMeshAgent _navMeshAgent;
    private BT_Tree StealingTree = new BT_Tree("Stealing");
    private BT_Leaf GoDiamond;
    private BT_Leaf TakeItemBackToVan;
    private BT_Leaf CheckMoney;
    private BT_Leaf GoSunflowers;
    private BT_Leaf GoFrontDoor;
    private BT_Leaf GoBackDoor;
    private BT_Node.NodeStatus _treeStatus = BT_Node.NodeStatus.RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        
        CommponentsSetup();
        BehaviourTreeSetup();

        void CommponentsSetup()
        {
            if(!TryGetComponent<NavMeshAgent>(out _navMeshAgent))
                Debug.LogError("Nav Mesh Agent not setup !");
        }
        
        void BehaviourTreeSetup()
        {

            BT_Sequence StealSequence = new BT_Sequence("Steal something");
            BT_Selector OpenDoor = new BT_Selector("Go pick a door");
            GoDiamond = new BT_Leaf("Go to diamond", GoToDiamond);
            GoSunflowers = new BT_Leaf("Go to sunflowers", GoToSunflowers);
            TakeItemBackToVan = new BT_Leaf("Go Back To Van", GoToVan);
            CheckMoney = new BT_Leaf("Go Back To Van", HasMoney);
            GoFrontDoor = new BT_Leaf("Go to the front door", GoToFrontDoor);
            GoBackDoor = new BT_Leaf("Go to the back door", GoToBackDoor);

            //StealingTree.AddChild(OpenDoor);
            OpenDoor.AddChild(GoFrontDoor);
            OpenDoor.AddChild(GoBackDoor);
            StealingTree.AddChild(StealSequence);
            StealSequence.AddChild(CheckMoney);
            StealSequence.AddChild(OpenDoor);
            StealSequence.AddChild(GoDiamond);
            StealSequence.AddChild(TakeItemBackToVan);
            StealSequence.AddChild(CheckMoney);
            StealSequence.AddChild(GoSunflowers);
            StealSequence.AddChild(TakeItemBackToVan);
            StealSequence.AddChild(CheckMoney);
           
            // Check the tree
            StealingTree.PrintTree();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (_treeStatus != BT_Node.NodeStatus.FAILURE)
            _treeStatus = StealingTree.Process();
        else
        {
            Debug.Log("Tree not processing : " + _treeStatus);
        }
        
    }
    
    private BT_Node.NodeStatus GoToVan()
    {
        BT_Node.NodeStatus s = GoToDestination(_vanTransform.position);

        if (s == BT_Node.NodeStatus.SUCCESS)
        {
            _currentMoney += _lastStolenItem.Prize;
        }

        return s;
    }

    private BT_Node.NodeStatus GoToDiamond()
    {
        return GoAndSteal(_diamond);
    }
    
    private BT_Node.NodeStatus GoToSunflowers()
    {
        return GoAndSteal(_sunflowers);
    }
    
    private BT_Node.NodeStatus GoToFrontDoor()
    {
        return GoToADoor(_frontDoor);
    }
    private BT_Node.NodeStatus GoToBackDoor()
    {
        return GoToADoor(_backDoor);
    }
    private BT_Node.NodeStatus HasMoney()
    {
        if (_currentMoney > _moneyAchievment)
            return BT_Node.NodeStatus.FAILURE;
        else
            return BT_Node.NodeStatus.SUCCESS;
    }

    private BT_Node.NodeStatus GoToADoor(OpenableDoor openableDoor)
    {
        BT_Node.NodeStatus s = GoToDestination(openableDoor.transform.position);

        if (s == BT_Node.NodeStatus.SUCCESS)
        {
            if (!openableDoor.IsLocked)
            {
                openableDoor.Open();
                return BT_Node.NodeStatus.SUCCESS;
            }
            return BT_Node.NodeStatus.FAILURE;
        }
        else
        {
            return s;
        }
        
    }
    private BT_Node.NodeStatus GoAndSteal(StealableItem stealableItem)
    {
        BT_Node.NodeStatus s = GoToDestination(stealableItem.transform.position);

        if (s == BT_Node.NodeStatus.SUCCESS)
        {
            stealableItem.StealItem();
            _lastStolenItem = stealableItem;
        }

        return s;
        
    }
    private BT_Node.NodeStatus GoToDestination(Vector3 destination)
    {
        
        //if(_navMeshAgent.velocity.magnitude < Mathf.Epsilon)
        if (Vector3.Distance(transform.position, destination) < _distanceSuccess)
        {
            // The node is at destination, so stop
            Debug.Log("Node arrived at destination");
            StealingTree.CurrentChild.Status =  BT_Node.NodeStatus.SUCCESS;
        }
        else
        {
             if (Vector3.Distance(destination, _navMeshAgent.destination) > Mathf.Epsilon)
             //if (StealingTree.CurrentChild.Status != BT_Node.NodeStatus.RUNNING)
                 _navMeshAgent.SetDestination(destination);
 
             // the node is not yet at destination
             StealingTree.CurrentChild.Status = BT_Node.NodeStatus.RUNNING;               
        }

        return StealingTree.CurrentChild.Status;
        
    }




}
