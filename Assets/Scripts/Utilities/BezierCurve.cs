using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints; // Array of empty GameObjects as control points

    public Vector3 GetPoint(float t)
    {
        return Bezier.GetPoint(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, t);
    }

    public Vector3 GetVelocity(float t)
    {
        return Bezier.GetFirstDerivative(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, t) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
}

public static class Bezier
{
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
               3f * oneMinusT * oneMinusT * t * p1 +
               3f * oneMinusT * t * t * p2 +
               t * t * t * p3;
    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return 3f * oneMinusT * oneMinusT * (p1 - p0) +
               6f * oneMinusT * t * (p2 - p1) +
               3f * t * t * (p3 - p2);
    }
}
