using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : MonoBehaviour
{
    public enum Deceleration
    {
        Slow = 3, Normal = 2, Fast = 1
    }
    public Deceleration deceleration;
    public float maxSpeed;
    public float panicRange;
    public float turnRate;
    public MovingEntity target;
}
