using UnityEngine;

public class TankMovement : MonoBehaviour
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    public float currentSpeed;
    public float currentAngularSpeed;

    void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {
        CalculateSpeed();
    }

    void CalculateSpeed()
    {
        // Calculate linear speed
        Vector3 displacement = transform.position - lastPosition;
        currentSpeed = displacement.magnitude / Time.deltaTime;
        lastPosition = transform.position;

        // Calculate angular speed
        Quaternion rotationDifference = transform.rotation * Quaternion.Inverse(lastRotation);
        float angle;
        Vector3 axis;
        rotationDifference.ToAngleAxis(out angle, out axis);
        currentAngularSpeed = angle * Mathf.Deg2Rad / Time.deltaTime;
        lastRotation = transform.rotation;
    }
}
