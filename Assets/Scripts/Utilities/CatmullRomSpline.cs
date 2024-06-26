using UnityEngine;

public class CatmullRomSpline : MonoBehaviour
{
    public Transform[] controlPoints;

    public Vector3 GetPoint(float t)
    {
        int numSections = controlPoints.Length;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        float u = t * numSections - currPt;

        Vector3 a = controlPoints[WrapIndex(currPt - 1)].position;
        Vector3 b = controlPoints[WrapIndex(currPt)].position;
        Vector3 c = controlPoints[WrapIndex(currPt + 1)].position;
        Vector3 d = controlPoints[WrapIndex(currPt + 2)].position;

        return 0.5f * (
            (-a + 3f * b - 3f * c + d) * (u * u * u) +
            (2f * a - 5f * b + 4f * c - d) * (u * u) +
            (-a + c) * u +
            2f * b
        );
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    private Vector3 GetVelocity(float t)
    {
        int numSections = controlPoints.Length;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        float u = t * numSections - currPt;

        Vector3 a = controlPoints[WrapIndex(currPt - 1)].position;
        Vector3 b = controlPoints[WrapIndex(currPt)].position;
        Vector3 c = controlPoints[WrapIndex(currPt + 1)].position;
        Vector3 d = controlPoints[WrapIndex(currPt + 2)].position;

        return 1.5f * (-a + 3f * b - 3f * c + d) * (u * u) +
               (2f * a - 5f * b + 4f * c - d) * u +
               0.5f * (c - a);
    }

    private int WrapIndex(int index)
    {
        int length = controlPoints.Length;
        return (index % length + length) % length;
    }
}
