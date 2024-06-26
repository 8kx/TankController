using UnityEngine;
using System.Collections.Generic;

public class TankTracks : MonoBehaviour
{
    public CatmullRomSpline originalSpline;
    public GameObject trackSegmentPrefab;
    public int segmentCount = 20;
    [HideInInspector] public GameObject[] trackSegments;
    public CatmullRomSpline newSpline; // Make it public

    void Start()
    {
        if (originalSpline == null)
        {
            Debug.LogError("Original spline is not assigned.");
            return;
        }

        trackSegments = new GameObject[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            trackSegments[i] = Instantiate(trackSegmentPrefab, transform);
            trackSegments[i].transform.parent = transform; // Parent to the tank object
        }

        List<Vector3> evenlySpacedPoints = GetEvenlySpacedPoints();
        GameObject newSplineObject = new GameObject("NewSpline");
        newSplineObject.transform.parent = transform; // Parent to the tank object
        newSpline = newSplineObject.AddComponent<CatmullRomSpline>();
        newSpline.controlPoints = new Transform[evenlySpacedPoints.Count];
        for (int i = 0; i < evenlySpacedPoints.Count; i++)
        {
            GameObject point = new GameObject("Point" + i);
            point.transform.position = evenlySpacedPoints[i];
            point.transform.parent = newSplineObject.transform; // Parent to the new spline object
            newSpline.controlPoints[i] = point.transform;
        }
    }

    public List<Vector3> GetEvenlySpacedPoints()
    {
        List<Vector3> points = new List<Vector3>();
        float totalLength = GetSplineLength();
        float interval = totalLength / (segmentCount - 1);

        float distanceTraveled = 0;
        points.Add(originalSpline.GetPoint(0));
        for (int i = 1; i < segmentCount; i++)
        {
            distanceTraveled += interval;
            points.Add(GetPointAtDistance(distanceTraveled));
        }

        return points;
    }

    public float GetSplineLength()
    {
        float length = 0;
        Vector3 prevPoint = originalSpline.GetPoint(0);
        int steps = 100; // Increase for more accuracy
        for (int i = 1; i <= steps; i++)
        {
            Vector3 point = originalSpline.GetPoint(i / (float)steps);
            length += Vector3.Distance(prevPoint, point);
            prevPoint = point;
        }
        return length;
    }

    public Vector3 GetPointAtDistance(float distance)
    {
        float length = 0;
        Vector3 prevPoint = originalSpline.GetPoint(0);
        int steps = 100; // Increase for more accuracy
        for (int i = 1; i <= steps; i++)
        {
            Vector3 point = originalSpline.GetPoint(i / (float)steps);
            float segmentLength = Vector3.Distance(prevPoint, point);
            if (length + segmentLength >= distance)
            {
                float t = (distance - length) / segmentLength;
                return Vector3.Lerp(prevPoint, point, t);
            }
            length += segmentLength;
            prevPoint = point;
        }
        return originalSpline.GetPoint(1); // Return the last point if distance exceeds total length
    }
}
