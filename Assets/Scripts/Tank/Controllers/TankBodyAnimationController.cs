using UnityEngine;

public class TankBodyAnimationController : MonoBehaviour
{
    public float frequency = 1.0f;
    public float damping = 0.5f;
    public float response = 1.0f;
    public float multiplier = 1.0f; // Multiplier for the results\
    public float externalForceMultiplier = 1.0f; // Multiplier for external forces
    public Vector3 invertedAxis = new Vector3(1, 1, 1); // Inverted axis for the input
    public Vector3 debugResult; // Public debug vector3 to see the result in runtime
    public Vector3 initialPosition;
    public float maxRot = 10.0f; // Max movement delta for X axis

    private SecondOrderDynamics dynamics;
    private Vector3 externalForces;
    private Vector3 lastPosition;

    void Start()
    {
        dynamics = new SecondOrderDynamics(frequency, damping, response, initialPosition);
        lastPosition = transform.position;
    }

    void Update()
    {
        dynamics.UpdateParameters(frequency, damping, response);

        // Calculate movement delta in local space
        Vector3 currentPosition = transform.position;
        Vector3 movementDelta = transform.InverseTransformDirection(currentPosition - lastPosition);
        lastPosition = currentPosition;

        // Apply external forces in local space
        Vector3 input = movementDelta + externalForces;

        // Update dynamics in local space
        Vector3 result = dynamics.Update(Time.deltaTime, input);

        // Clamp the movement delta
        result.x = Mathf.Clamp(result.x, -maxRot, maxRot);
        result.z = Mathf.Clamp(result.z, -maxRot, maxRot);

        // Apply multiplier
        result *= multiplier;

        // Set debug result for runtime viewing
        debugResult = result;

        // Apply the local result to the rotation
        Vector3 localEulerAngles = transform.localEulerAngles;
        localEulerAngles.x = -result.z;
        localEulerAngles.z = -result.x;
        transform.localEulerAngles = localEulerAngles;

        // Reset external forces after applying
        externalForces = Vector3.zero;
    }

    // Call this method to add external forces (e.g., gun firing)
    public void AddExternalForce(Vector3 force)
    {
        force *= externalForceMultiplier;
        externalForces += transform.InverseTransformDirection(force);
        externalForces = MultiplyVectors(externalForces, invertedAxis);
    }

        // Method to multiply vectors component-wise
    private Vector3 MultiplyVectors(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}
