using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT_Tree_Workshop
{
    public class Robber : MonoBehaviour
    {

        [SerializeField] private Transform _vanPosition;
        [SerializeField] private Transform _diamondPosition;
        [SerializeField] private Transform _doorPosition;

        private BT_Root _tree = new BT_Root("Robber Behaviour Tree");
        private NavMeshAgent _navMeshAgent;

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            BT_Leaf goToVanLeaf = new BT_Leaf("Go to van", () => GoToDestination(_vanPosition));
            BT_Leaf goToDiamondLeaf = new BT_Leaf("Go to Diamond", () => GoToDestination(_diamondPosition));
            BT_Leaf goToDoorLeaf = new BT_Leaf("Go to Door", () => GoToDestination(_doorPosition));
            BT_Sequence stealSequence = new BT_Sequence("Payday robbery");
            
            stealSequence.AddChild(goToDoorLeaf);
            stealSequence.AddChild(goToDiamondLeaf);
            stealSequence.AddChild(goToVanLeaf);
            _tree.AddChild(stealSequence);
                        
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
            BT_Status status = _tree.Process();
            Debug.Log("Robber status : " + status);
        }
    }
}
