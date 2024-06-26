using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{
    public float maxMoveSpeed = 5f;
    public float acceleration = 2f;
    public float deceleration = 1f; // Deceleration when no input
    public float drag = 0.95f;

    [Header("Second Order Dynamics Parameters")]
    public float frequency = 1.0f;
    public float damping = 0.5f;
    public float response = 1.0f;

    private TankControls controls;
    private Vector2 rawMovementInput;
    private float currentSpeed = 0f;
    private bool isReversing = false;
    private Vector3 currentDirection = new Vector3(0, 0, 1); // Initial direction

    private SecondOrderDynamics dynamics;
    public float angleMulti = 1.0f;

    private void Awake()
    {
        controls = new TankControls();

        // Initialize SecondOrderDynamics with exposed parameters
        dynamics = new SecondOrderDynamics(frequency, damping, response, 0f);

        // Register the callback methods for the actions
        controls.Tank.Move.performed += ctx => rawMovementInput = ctx.ReadValue<Vector2>();
        controls.Tank.Move.canceled += ctx => rawMovementInput = Vector2.zero;

        controls.Tank.Reverse.performed += ctx => isReversing = ctx.ReadValueAsButton();
        controls.Tank.Reverse.canceled += ctx => isReversing = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        // Update SecondOrderDynamics parameters at runtime
        dynamics.UpdateParameters(frequency, damping, response);

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 direction = new Vector3(rawMovementInput.x, 0, rawMovementInput.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle;
            if (isReversing)
            {
                // Rotate towards the opposite direction for visual reverse
                targetAngle = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg;
            }
            else
            {
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            }

            float currentAngle = transform.eulerAngles.y;
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

            // Speed up the rotation
            angleDifference *= angleMulti;

            // Use SecondOrderDynamics to smooth angular acceleration
            float angularAcceleration = dynamics.Update(Time.deltaTime, angleDifference);

            // Apply the smoothed angular acceleration to the tank's rotation
            float smoothedAngle = currentAngle + angularAcceleration * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);
        }

        // Accelerate the tank only when there is user input
        if (rawMovementInput.magnitude > 0)
        {
            float speedMultiplier = isReversing ? -1 : 1;
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxMoveSpeed * speedMultiplier, acceleration * Time.deltaTime);
        }
        else
        {
            // Decelerate the tank when there is no input
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Move the tank
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        // Draw a line to show the direction and speed of the tank
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * currentSpeed);
    }
}
