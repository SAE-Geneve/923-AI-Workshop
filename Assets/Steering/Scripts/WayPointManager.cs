using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    
    // Waypoints
    [SerializeField] private List<WayPoint> _waypoints;

    public List<WayPoint> Waypoints => _waypoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
