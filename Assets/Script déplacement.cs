using UnityEngine;
using UnityEngine.InputSystem;

public class SubmarineController : MonoBehaviour
{
    public float moveSpeed = 1f; 
    public float rotateSpeed = 100f;
    private float currentSpeed;
    private Rigidbody rb;
    private Animator animator;
    private InputAction moveForwardAction;
    private InputAction moveBackwardAction;
    private InputAction descendAction;
    private InputAction ascendAction;
    private InputAction shiftAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

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
        currentSpeed = shiftAction.ReadValue<float>() > 0.1f ? moveSpeed * 2f : moveSpeed;

        Vector3 movement = Vector3.zero;
        float direction = 0f; 

        if (moveForwardAction.ReadValue<float>() > 0.1f)
        {
            movement += transform.forward * currentSpeed * Time.deltaTime;
            direction = 1f;
        }
        if (moveBackwardAction.ReadValue<float>() > 0.1f)
        {
            movement -= transform.forward * currentSpeed * Time.deltaTime;
            direction = -1f;
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

        UpdateAnimator(direction);
    }

    private void UpdateAnimator(float direction)
    {
        bool isMoving = direction != 0 || ascendAction.ReadValue<float>() > 0.1f || descendAction.ReadValue<float>() > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("Speed", currentSpeed);
            animator.SetFloat("Direction", direction);
            
            if (shiftAction.ReadValue<float>() > 0.1f)
                animator.Play("AnimHelicesGrandes");
            else
                animator.Play("AnimHelicesPetites");
        }
        else
        {
            animator.Play("AnimRepos");
        }
    }
}