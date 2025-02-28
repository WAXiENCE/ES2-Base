using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Lien avec le système d'entrée de Unity

public class SubmarineController : MonoBehaviour
{
    // Variables pour le mouvement
    public float moveSpeed = 5f;
    private float currentSpeed; 
  

    private InputAction moveForwardAction;
    private InputAction moveBackwardAction;
    private InputAction descendAction;
    private InputAction ascendAction;
    private InputAction shiftAction;

    private Rigidbody rb; 

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody>();

        
        var inputActions = new InputActionMap("Submarine");

        
        moveForwardAction = inputActions.AddAction("MoveForward", binding: "<Keyboard>/w");
        moveBackwardAction = inputActions.AddAction("MoveBackward", binding: "<Keyboard>/s");
        descendAction = inputActions.AddAction("Descend", binding: "<Keyboard>/q");
        ascendAction = inputActions.AddAction("Ascend", binding: "<Keyboard>/e");

        
        shiftAction = inputActions.AddAction("Shift", binding: "<Keyboard>/shift");

        
        inputActions.Enable();
        
        
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        
        if (shiftAction.ReadValue<float>() > 0.1f) 
        {
            currentSpeed = moveSpeed * 2f; 
        }
        else
        {
            currentSpeed = moveSpeed;
        }


        Vector3 movement = Vector3.zero;

        if (moveForwardAction.ReadValue<float>() > 0.1f)
        {
            movement += transform.forward * currentSpeed * Time.deltaTime;
        }

        if (moveBackwardAction.ReadValue<float>() > 0.1f)
        {
            movement -= transform.forward * currentSpeed * Time.deltaTime;
        }

        if (descendAction.ReadValue<float>() > 0.1f)
        {
            movement -= transform.up * currentSpeed * Time.deltaTime;
        }

        if (ascendAction.ReadValue<float>() > 0.1f)
        {
            movement += transform.up * currentSpeed * Time.deltaTime;
        }

        rb.MovePosition(rb.position + movement);

    }

}