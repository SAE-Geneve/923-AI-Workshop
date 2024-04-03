using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SteeringBehavior : MonoBehaviour
{

    [SerializeField] private float _maxVelocity;

    [Header("Seek")] [SerializeField] private Transform _seekTarget;
    [SerializeField] private float _seekWeight;

    [Header("Evade")] [SerializeField] private Transform _evadeTarget;
    [SerializeField] private float _evadeWeight;
    [SerializeField] private float _evadeDistance;

    [Header("Wander")] 
    [SerializeField] private float _wanderWeight = 1;
    [SerializeField] private float _wanderCircleDistance = 1;
    [SerializeField] private float _wanderCircleRadius = 2;
    [SerializeField] private float _wanderSpeed;
    
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
        // _acceleration += _seekWeight * Seek(_seekTarget);
        // if (Vector3.Distance(_evadeTarget.position, transform.position) <= _evadeDistance)
        // {
        //     _acceleration += _evadeWeight * Evade(_evadeTarget);
        // }
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
        Vector3 steeringForce = Vector3.zero;

        // Calculate the circle center
        Vector3 circleCenter = transform.position + _rb.velocity.normalized * _wanderCircleDistance;

        // Generate a random angle
        _wanderAngle += Random.Range(-1f*_wanderSpeed, _wanderSpeed);

        // Convert the angle to a vector on the circle
        Vector3 wanderDirection = new Vector3(Mathf.Sin(_wanderAngle), 0, Mathf.Cos(_wanderAngle));

        // Calculate the target position on the circle
        Vector3 targetPosition = circleCenter + wanderDirection * _wanderCircleRadius;

        // Calculate the steering force towards the target position
        _wanderVelocity = targetPosition - transform.position;
        
        steeringForce = _wanderVelocity - _rb.velocity;
        // Return the steering force (you may want to normalize it based on your requirements)
        return steeringForce;
    }


}
