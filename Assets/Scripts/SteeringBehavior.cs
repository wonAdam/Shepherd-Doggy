using System;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{
    public enum SteeringBehaviorState
    {
        Seek, Flee, Arrive, Pursuit, Cohesion, Separation
    }
    protected MovingEntity movingEntity;
    protected void Start()
    {
        movingEntity = GetComponent<MovingEntity>();
    }

    [SerializeField] public float groupBehaviorRadius;

    public virtual Vector3 Calculate(SteeringBehaviorState state, Vector3 target)
    {
        switch (state)
        {
            case SteeringBehaviorState.Arrive:
                {
                    return Arrive(target);
                }
            case SteeringBehaviorState.Flee:
                {
                    return Flee(target);
                }
            case SteeringBehaviorState.Seek:
                {
                    return Seek(target);
                }
            case SteeringBehaviorState.Pursuit:
                {
                    return Pursuit(movingEntity.target);
                }
            case SteeringBehaviorState.Cohesion:
                {
                    List<MovingEntity> neighbors = TagNeighbors(movingEntity, NeighborPredicate);
                    return Cohesion(movingEntity, neighbors);
                }
            case SteeringBehaviorState.Separation:
                { 
                    List<MovingEntity> neighbors = TagNeighbors(movingEntity, NeighborPredicate);
                    return Separation(movingEntity, neighbors);
                }
                
            default:
                return Vector3.zero;

        }
    }

    protected Vector3 Seek(Vector3 target)
    {
        Vector3 desiredVelocity = (movingEntity.target.transform.position - transform.position).normalized * movingEntity.maxSpeed;
        return desiredVelocity - movingEntity.GetComponent<Rigidbody>().velocity;
    }

    protected Vector3 Flee(Vector3 target)
    {
        if (Vector3.Distance(movingEntity.target.transform.position, transform.position) > movingEntity.panicRange) return Vector3.zero;

        Vector3 desiredVelocity = (transform.position - movingEntity.target.transform.position).normalized * movingEntity.maxSpeed;
        return desiredVelocity - movingEntity.GetComponent<Rigidbody>().velocity;
    }

    [Header("Arrive")]
    public float _dist;
    public Vector3 _arrive_desiredVelocity;
    protected Vector3 Arrive(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;

        float dist = toTarget.magnitude;
        _dist = dist;
        if (dist > 0)
        {
            const float decelerationTweaker = 0.3f;
            float speed = dist / ((float)movingEntity.deceleration * decelerationTweaker);


            speed = Mathf.Min(speed, movingEntity.maxSpeed);

            Vector3 desiredVelocity = toTarget.normalized * speed;
            _arrive_desiredVelocity = desiredVelocity;

            return desiredVelocity - movingEntity.GetComponent<Rigidbody>().velocity;
        }

        return Vector3.zero;
    }

    public Vector3 _pursuit_expected;
    protected Vector3 Pursuit(MovingEntity target)
    {
        Vector3 toEvader = movingEntity.target.transform.position - transform.position;

        float relativeHeading = Vector3.Dot(transform.forward, movingEntity.target.transform.forward);

        if(Vector3.Dot(toEvader, transform.forward) > 0 &&
            relativeHeading < Mathf.Cos(150f))
        {
            return Seek(target.transform.position);
        }

        float lookAheadTime = toEvader.magnitude / (movingEntity.target.GetComponent<Rigidbody>().velocity.magnitude + movingEntity.maxSpeed);

        _pursuit_expected = target.transform.position + target.GetComponent<Rigidbody>().velocity * lookAheadTime;
        return Seek(target.transform.position + target.GetComponent<Rigidbody>().velocity * lookAheadTime);
    }
    [SerializeField] Vector3 centerOfMass;
    protected Vector3 Cohesion(MovingEntity movingEntity, List<MovingEntity> neighbors)
    {
        if (neighbors.Count == 0) return Vector3.zero;

        Vector3 centerOfMass = Vector2.zero;

        foreach (var neighbor in neighbors)
            centerOfMass += neighbor.transform.position;

        centerOfMass /= neighbors.Count;
        this.centerOfMass = centerOfMass;
        return Arrive(centerOfMass);
    }

    [SerializeField] float separationFactor;
    protected Vector3 Separation(MovingEntity movingEntity, List<MovingEntity> neighbors)
    {
        Vector3 steeringForce = Vector3.zero;

        foreach (var neighbor in neighbors)
        {
            Vector3 ToAgent = transform.position - neighbor.transform.position;

            steeringForce += ToAgent.normalized / ToAgent.magnitude;
        }

        return steeringForce.normalized * movingEntity.maxSpeed;
    }

    [SerializeField] List<MovingEntity> neighbors = new List<MovingEntity>();
    protected List<MovingEntity> TagNeighbors(MovingEntity movingEntity, Predicate<MovingEntity> IsNeighbor)
    {
        List<MovingEntity> result = new List<MovingEntity>();
        Collider[] colliders = Physics.OverlapSphere(movingEntity.transform.position, groupBehaviorRadius);

        foreach (var collider in colliders)
            if (collider.GetComponent<MovingEntity>() != null &&
                collider.GetComponent<MovingEntity>() != movingEntity &&
                IsNeighbor(collider.GetComponent<MovingEntity>()))
                result.Add(collider.GetComponent<MovingEntity>());

        neighbors = result;
        return result;
    }
    protected bool NeighborPredicate(MovingEntity neighborMovingEntity) => neighborMovingEntity.gameObject.layer == gameObject.layer;

}
