using UnityEngine;

public class OverlapAnimation : MonoBehaviour
{
    public TankMovement tankMovement; // Reference to the tank movement script
    public float baseFrequency = 1.0f; // Base frequency of the sine wave
    public float baseAmplitude = 10.0f; // Base amplitude of the sine wave
    public float frequencyMultiplier = 0.1f; // Multiplier for frequency based on speed
    public float amplitudeMultiplier = 0.1f; // Multiplier for amplitude based on speed
    public bool applyToXAxis = false; // Apply sine wave rotation to X axis
    public bool applyToYAxis = true; // Apply sine wave rotation to Y axis
    public bool applyToZAxis = false; // Apply sine wave rotation to Z axis

    private Vector3 initialRotation;

    private void Start()
    {
        initialRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        float speed = Mathf.Abs(tankMovement.currentSpeed); // Get the absolute value of the current speed

        // Calculate the frequency and amplitude based on the speed
        float frequency = baseFrequency + (speed * frequencyMultiplier);
        float amplitude = baseAmplitude + (speed * amplitudeMultiplier);

        // Apply the sine wave rotation
        Vector3 newRotation = initialRotation;
        float sineValue = Mathf.Sin(Time.time * frequency) * amplitude;

        if (applyToXAxis)
        {
            newRotation.x += sineValue;
        }
        if (applyToYAxis)
        {
            newRotation.y += sineValue;
        }
        if (applyToZAxis)
        {
            newRotation.z += sineValue;
        }

        transform.localRotation = Quaternion.Euler(newRotation);
    }
}
