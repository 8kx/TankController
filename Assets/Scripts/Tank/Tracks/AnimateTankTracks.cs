using UnityEngine;

public class AnimateTankTracks : MonoBehaviour
{
    public TankTracks tankTracks;
    public TankMovement tankMovement;
    public float baseSpeed = 1.0f;
    public float trackRadius = 1.0f; // The radius from the center of the tank to the tracks

    private float leftOffset = 0f;
    private float rightOffset = 0f;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float speed = tankMovement.currentSpeed * baseSpeed;
        float angularSpeed = tankMovement.currentAngularSpeed * baseSpeed;

        // Determine if the tank is moving forward or in reverse based on position change
        Vector3 currentPosition = transform.position;
        Vector3 movementDirection = currentPosition - lastPosition;
        bool isReversing = Vector3.Dot(movementDirection, transform.forward) < 0;

        // Reverse the speed if the tank is moving in reverse
        if (isReversing)
        {
            speed = -speed;
        }

        lastPosition = currentPosition;

        // Calculate the track speeds
        float leftTrackSpeed = speed - angularSpeed * trackRadius;
        float rightTrackSpeed = speed + angularSpeed * trackRadius;

        leftOffset += leftTrackSpeed * Time.deltaTime;
        rightOffset += rightTrackSpeed * Time.deltaTime;

        if (leftOffset > 1f) leftOffset -= 1f;
        if (rightOffset > 1f) rightOffset -= 1f;
        if (leftOffset < -1f) leftOffset += 1f;
        if (rightOffset < -1f) rightOffset += 1f;

        GameObject[] trackSegments = tankTracks.trackSegments;
        for (int i = 0; i < trackSegments.Length; i++)
        {
            float t = i / (float)(trackSegments.Length - 1);

            // Adjust offsets for left and right tracks
            float leftT = t + leftOffset;
            float rightT = t + rightOffset;
            if (leftT > 1f) leftT -= 1f;
            if (rightT > 1f) rightT -= 1f;
            if (leftT < 0f) leftT += 1f;
            if (rightT < 0f) rightT += 1f;

            // Calculate the position of the left and right track points
            Vector3 leftTrackPoint = tankTracks.newSpline.GetPoint(leftT);
            Vector3 rightTrackPoint = tankTracks.newSpline.GetPoint(rightT);

            // Calculate the offset from the tank's center to the left and right tracks
            Vector3 leftOffsetPosition = transform.right * -trackRadius;
            Vector3 rightOffsetPosition = transform.right * trackRadius;

            // Set the position and rotation for the left and right track segments
            if (i % 2 == 0) // Assuming alternating segments for left and right
            {
                trackSegments[i].transform.position = leftTrackPoint + leftOffsetPosition;
                trackSegments[i].transform.rotation = Quaternion.LookRotation(tankTracks.newSpline.GetDirection(leftT));
            }
            else
            {
                trackSegments[i].transform.position = rightTrackPoint + rightOffsetPosition;
                trackSegments[i].transform.rotation = Quaternion.LookRotation(tankTracks.newSpline.GetDirection(rightT));
            }
        }
    }
}
