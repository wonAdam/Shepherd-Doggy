using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : Controller
{
    // Update is called once per frame
    void Update()
    {
        Vector2 WASDInput = GetInput();
        mover.ProcessInput(WASDInput);
    }
    private Vector2 GetInput()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        return new Vector2(h, v);
    }
}
