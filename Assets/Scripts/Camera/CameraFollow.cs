using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow, usually the tank
    public Vector3 offset; // Offset position from the target
    public float frequency = 1.0f; // Frequency for second-order dynamics
    public float damping = 0.5f; // Damping for second-order dynamics
    public float response = 1.0f; // Response for second-order dynamics

    private SecondOrderDynamics dynamics;
    private Vector3 smoothedPosition;

    private void Start()
    {
        // Initialize the SecondOrderDynamics with the current position as the starting point
        dynamics = new SecondOrderDynamics(frequency, damping, response, transform.position);

        transform.position = target.position + offset;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the target position with the offset
        Vector3 targetPosition = target.position + offset;

        // Smooth the position using SecondOrderDynamics
        smoothedPosition = dynamics.Update(Time.deltaTime, targetPosition);

        // Update the camera's position
        transform.position = smoothedPosition;
    }

    private void OnValidate()
    {
        // Update SecondOrderDynamics parameters in case they are changed in the Inspector
        if (dynamics != null)
        {
            dynamics.UpdateParameters(frequency, damping, response);
        }
    }
}
