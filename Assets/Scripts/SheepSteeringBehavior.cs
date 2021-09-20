using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSteeringBehavior : SteeringBehavior
{
    [SerializeField] float clampWeight;
    [SerializeField] float fleeWeight;
    [SerializeField] float cohesionWeight;
    [SerializeField] float separationWeight;

    private void Start()
    {
        base.Start();
    }
    public override Vector3 Calculate(SteeringBehaviorState state, Vector3 target)
    {
        Vector3 desiredDirect = Vector3.zero;

        if(desiredDirect.magnitude < movingEntity.maxSpeed)
        {
            desiredDirect += Flee(target) * fleeWeight;
        }

        List<MovingEntity> neighbors = TagNeighbors(movingEntity, NeighborPredicate);
        if (desiredDirect.magnitude < movingEntity.maxSpeed)
        {
            desiredDirect += Vector3.ClampMagnitude(
                Cohesion(movingEntity, neighbors) * fleeWeight, 
                movingEntity.maxSpeed - desiredDirect.magnitude);
        }

        if (desiredDirect.magnitude < movingEntity.maxSpeed)
        {

            desiredDirect += Vector3.ClampMagnitude(
                Separation(movingEntity, neighbors) * separationWeight,
                movingEntity.maxSpeed - desiredDirect.magnitude);
        }

        return desiredDirect;
    }
}
