// This code is to control the ROV with the PS4 controller, inspired from: https://www.youtube.com/watch?v=p-3S73MaDP8&ab_channel=Brackeys
//Author: Ahmed Harbi
//Date: 2022-12-26
//Email: ahmedharbii10@gmail.com

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
        Vector3 m = 0.2f * Time.deltaTime * new Vector3(move.y, 0 , move.x);
        transform.Translate(m, Space.Self); //); //the world space
        GetComponent<Rigidbody>().MovePosition(transform.position + m); // * 50f *

        // * 50f *
        Vector2 r = 10f * Time.deltaTime * new Vector2(rotate.y, -rotate.x); //100f to make it quicker
        transform.Rotate(r, Space.Self); //the world space
        // GetComponent<Rigidbody>().MovePosition

    }

    void MoveUp()
    {
        transform.Translate(0, -0.05f * Time.deltaTime, 0);
    }
    
    void MoveDown()
    {
        transform.Translate(0, 0.05f * Time.deltaTime, 0);
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

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class rov : MonoBehaviour
// {
//     RovControls controls;
//     Vector2 move;
//     Vector2 rotate;

//     void Awake() //called even before start
//     {
//         controls = new RovControls();
//         // Use a single event handler for the Move and Rotate actions
//         controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
//         controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
//     }

//     void FixedUpdate()
//     {
//         // Multiply by Time.unscaledDeltaTime to make it framerate independent
//         Vector3 m = 0.2f * Time.unscaledDeltaTime * new Vector3(move.y, 0 , move.x);
//         GetComponent<Rigidbody>().MovePosition(transform.position + m); // Use Rigidbody.MovePosition() instead of Transform.Translate()

//         // * 50f *
//         Vector2 r = 10f * Time.unscaledDeltaTime * new Vector2(rotate.y, -rotate.x); //100f to make it quicker
//         transform.Rotate(r, Space.Self); //the world space

//     }

//     void OnEnable()
//     {
//         controls.Gameplay.Enable();
//     }

//     void OnDisable()
//     {
//         controls.Gameplay.Disable();
//     }
// }
