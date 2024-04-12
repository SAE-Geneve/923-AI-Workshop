using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTrees;
using UnityEngine;
using UnityEngine.AI;

namespace BT_Tree_Workshop
{
    public class Robber : MonoBehaviour
    {

        [SerializeField] private Transform _vanPosition;
        [SerializeField] private List<StealableItem> _stealableItems;
        [SerializeField] private OpenableDoor _frontDoor;
        [SerializeField] private OpenableDoor _backDoor;

        private BT_Root _tree = new BT_Root("Robber Behaviour Tree");
        private NavMeshAgent _navMeshAgent;

        private int _money = 0;
        private StealableItem _stolenItem;
        private BT_Status _treeStatus;

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            BT_Leaf goToFrontDoorLeaf = new BT_Leaf("Go to Front Door", () => DoorAction(_frontDoor));
            BT_Leaf goToBackDoorLeaf = new BT_Leaf("Go to Back Door", () => DoorAction(_backDoor));
            // BT_Leaf goToDiamondLeaf = new BT_Leaf("Go to Diamond", () => GoToDestination(_diamond.transform));
            // BT_Leaf stealDiamond = new BT_Leaf("Steal Diamond", () => StealSomething(_diamond));
            BT_Leaf goToVanLeaf = new BT_Leaf("Go to van", () => GoToDestination(_vanPosition));
            BT_Leaf collectMoney = new BT_Leaf("Collect Money", () => CollectMoney());
            
            BT_Sequence stealSequence = new BT_Sequence("Payday robbery");
            BT_Selector doorSelector = new BT_Selector("Pick a door");
            
            stealSequence.AddChild(doorSelector);
            foreach (var stealableItem in _stealableItems.OrderByDescending(i => i.Prize))
            {
                stealSequence.AddChild(new BT_Leaf("Go to " + stealableItem.name, () => GoToDestination(stealableItem.transform)));
                stealSequence.AddChild(new BT_Leaf("Steal " + stealableItem.name, () => StealSomething(stealableItem)));
                stealSequence.AddChild(goToVanLeaf);
                stealSequence.AddChild(collectMoney);
            }
            
            doorSelector.AddChild(goToFrontDoorLeaf);
            doorSelector.AddChild(goToBackDoorLeaf);
            
            _tree.AddChild(stealSequence);
                        
        }

        private BT_Status CollectMoney()
        {
            _money += _stolenItem.Prize;
            return BT_Status.Success;
        }

        private BT_Status StealSomething(StealableItem item)
        {
            item.StealItem();
            _stolenItem = item;
            return BT_Status.Success;
        }

        private BT_Status DoorAction(OpenableDoor door)
        {
            BT_Status status = GoToDestination(door.transform);
            
            if (status == BT_Status.Success)
            {
                if (door.IsLocked)
                {
                    door.NavMeshObstacle.carving = true;
                    return BT_Status.Failure;
                }
                else
                {
                    door.Open();
                    return BT_Status.Success;
                }
    
            }

            return status;


        }

        private BT_Status GoToDestination(Transform destination)
        {

            // Failure :
            // il y a pas de van (?)
            // On ne peut pas y aller
            // point technique
            if (_navMeshAgent is null || destination is null)
            {
                Debug.Log("Technical failure.");
                return BT_Status.Failure;
            }
            if (Vector3.Distance(transform.position, destination.position) > _navMeshAgent.stoppingDistance)
            {
                NavMeshPath path = new NavMeshPath();
                Debug.Log("Trying to go to " + destination.name);
                if (_navMeshAgent.CalculatePath(destination.position, path))
                {
                    _navMeshAgent.destination = destination.position;
                }
                else
                {
                    Debug.Log("Path failure.");
                    return BT_Status.Failure;
                }

                // Running : Is going to the van
                return BT_Status.Running;
            }
            else
            {
                // Succes : On est arrivé
                return BT_Status.Success;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_treeStatus != BT_Status.Failure)
                _treeStatus = _tree.Process();
            else
            {
                Debug.Log("Tree not processing : " + _treeStatus);
            }
        }
    }
}
