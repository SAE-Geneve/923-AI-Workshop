using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WayPointFollower : MonoBehaviour
{

    [SerializeField] private WayPointManager _wayPointManager;
    [SerializeField] private float _speed = 5f;
    [SerializeField] [Range(0f, 1f)] private float _lerpFactor = 0.5f;
    [SerializeField] private float _pointTolerance = 0.5f;

    private int _idxWayPoint = 0;

    private Vector3 destination;

    // Update is called once per frame
    void Update()
    {
        destination = _wayPointManager.Waypoints[_idxWayPoint].transform.position;
        Debug.DrawRay(transform.position, destination - transform.position, Color.blue);

        Quaternion destinationRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, destinationRotation, _lerpFactor);

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        if (Vector3.Distance(destination, transform.position) < _pointTolerance)
        {
            _idxWayPoint++;
            if (_idxWayPoint >= _wayPointManager.Waypoints.Count)
                _idxWayPoint = 0;
        }

    }


    private void OnDrawGizmos()
    {
        foreach (var wayPoint in _wayPointManager.Waypoints)
        {
            if (wayPoint == _wayPointManager.Waypoints[_idxWayPoint])
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.white;

            Gizmos.DrawWireSphere(wayPoint.transform.position, _pointTolerance);
        }
    }
}
