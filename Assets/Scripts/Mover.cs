using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public enum Deceleration
    {
        Slow = 3, Medium = 2, Fast = 1
    }
    [SerializeField] public Deceleration deceleration = Deceleration.Slow;
    [SerializeField] public float speed;
    [SerializeField] public float rot;
    [SerializeField] protected Animator anim;
    [SerializeField] 

    public abstract void ProcessInput(Vector2 dir);
}
