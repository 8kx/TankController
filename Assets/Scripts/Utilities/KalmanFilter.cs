using UnityEngine;

public class KalmanFilter
{
    public float Q { get; set; } // Process noise covariance
    public float R { get; set; } // Measurement noise covariance
    private float P; // Estimate error covariance
    private float X; // Value

    public KalmanFilter(float q, float r, float initialP, float initialX)
    {
        Q = q;
        R = r;
        P = initialP;
        X = initialX;
    }

    public float Update(float measurement, float deltaTime)
    {
        // Prediction update (Semi-Implicit Euler)
        P = P + Q * deltaTime;

        // Measurement update
        float K = P / (P + R); // Kalman gain
        X = X + K * (measurement - X);
        P = (1 - K) * P;

        return X;
    }
}
