using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float rot;
    [SerializeField] protected Animator anim;

    public abstract void ProcessInput(Vector2 dir);
}
