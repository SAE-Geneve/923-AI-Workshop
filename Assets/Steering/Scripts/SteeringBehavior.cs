using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SteeringBehavior : MonoBehaviour
{

    [SerializeField] private float _maxVelocity;

    [Header("Seek")] 
    [SerializeField] private Transform _seekTarget;
    [SerializeField] private float _seekWeight;

    [Header("Evade")] 
    [SerializeField] private Transform _evadeTarget;
    [SerializeField] private float _evadeWeight;
    [SerializeField] private float _evadeDistance;

    [Header("Wander")] 
    [SerializeField] private float _wanderWeight = 1;
    [SerializeField] private float _wanderCircleDistance = 1;
    [SerializeField] private float _wanderCircleRadius = 2;
    [SerializeField] private float _wanderChange;
    
    private Rigidbody _rb;
    private Vector3 _acceleration;
    private Vector3 _seekDesiredVelocity;
    private Vector3 _evadeVelocity;
    private Vector3 _wanderVelocity;
    private float _wanderAngle;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = _rb.velocity;

        _acceleration = Vector3.zero;
        _acceleration += _seekWeight * Seek(_seekTarget);
        _acceleration += _evadeWeight * Evade(_evadeTarget);
        _acceleration += _wanderWeight * Wander();

        velocity += _acceleration * Time.deltaTime;

        if (velocity.magnitude > _maxVelocity)
            velocity = velocity.normalized * _maxVelocity;

        _rb.velocity = velocity;

    }

    private void OnDrawGizmos()
    {
        if (_rb != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, _rb.velocity);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, _seekDesiredVelocity);
            Gizmos.DrawRay(transform.position, _evadeVelocity);
            Gizmos.DrawRay(transform.position, _wanderVelocity);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + _rb.velocity, _acceleration);
        }

        if (_evadeTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_evadeTarget.position, _evadeDistance);
        }

    }

    private Vector3 Seek(Transform target)
    {
        Vector3 steeringForce = new Vector3();

        _seekDesiredVelocity = target.position - transform.position;
        steeringForce = _seekDesiredVelocity - _rb.velocity;

        return steeringForce;
    }

    private Vector3 Evade(Transform target)
    {
        Vector3 steeringForce = new Vector3();

        _evadeVelocity = transform.position - target.position;
        steeringForce = _evadeVelocity - _rb.velocity;

        return steeringForce;
    }

    private Vector3 Wander()
    {
        
        Vector3 steeringForce;;

        // Calculate the circle center
        Vector3 circleCenter = _wanderCircleDistance * _rb.velocity.normalized;
        
        // Calculate the displacement force
        Vector3 displacement = _wanderCircleRadius * Vector3.forward;
        
        //
        // Randomly change the vector direction
        // by making it change its current angle
        displacement = Quaternion.AngleAxis(_wanderAngle, Vector3.up) * displacement;
        
        //
        // Change wanderAngle just a bit, so it
        // won't have the same value in the
        // next game frame.
        _wanderAngle += Random.Range(-1 * _wanderChange , _wanderChange);

        //
        // Finally calculate and return the wander force
        Vector3 wanderTarget = circleCenter + displacement;
        
        _wanderVelocity = wanderTarget - transform.position;
        steeringForce = _wanderVelocity - _rb.velocity;

        return steeringForce;


    }


}
