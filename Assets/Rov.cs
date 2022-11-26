using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class rov : MonoBehaviour
{
    RovControls controls;
    Vector2 move;
    Vector2 rotate;

    void Awake() //called even before start
    {
        controls = new RovControls();
        // ctx: context - name it anything
        // => for lambda expression
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

    }


    void Update()
    {
        // Multiply by Time.deltaTime to make it framerate independent
        Vector2 m = Time.deltaTime * new Vector2(-move.x, move.y);
        transform.Translate(m, Space.World); //the world space

        Vector2 r = Time.deltaTime * 100f * new Vector2(-rotate.y, rotate.x); //100f to make it quicker
        transform.Rotate(r, Space.World); //the world space

    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
