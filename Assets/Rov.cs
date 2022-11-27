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
        controls.Gameplay.MoveUp.performed += ctx => MoveUp();
        controls.Gameplay.MoveDown.performed += ctx => MoveDown();
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
        Vector3 m = Time.deltaTime * new Vector3(-move.x, 0 , -move.y);
        transform.Translate(m, Space.World); //); //the world space

        // * 50f *
        Vector2 r = 30f * Time.deltaTime * new Vector2(rotate.y, rotate.x); //100f to make it quicker
        transform.Rotate(r, Space.World); //the world space

    }

    void MoveUp()
    {
        transform.Translate(0, -0.5f * Time.deltaTime, 0);
    }
    
    void MoveDown()
    {
        transform.Translate(0, 0.5f * Time.deltaTime, 0);
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
