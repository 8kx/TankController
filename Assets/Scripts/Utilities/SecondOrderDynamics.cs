using UnityEngine;

public class SecondOrderDynamics
{
    private float xp; // Previous input for scalar
    private float y, yd; // State variables for scalar
    private float k1, k2, k3; // Dynamics constants for scalar

    private Vector3 xpVec; // Previous input for vector
    private Vector3 yVec, ydVec; // State variables for vector
    private float k1Vec, k2Vec, k3Vec; // Dynamics constants for vector

    public SecondOrderDynamics(float f, float z, float r, float x0)
    {
        UpdateParameters(f, z, r);
        xp = x0;
        y = x0;
        yd = 0f;
    }

    public SecondOrderDynamics(float f, float z, float r, Vector3 x0)
    {
        UpdateParameters(f, z, r);
        xpVec = x0;
        yVec = x0;
        ydVec = Vector3.zero;
    }

    public void UpdateParameters(float f, float z, float r)
    {
        k1 = z / (Mathf.PI * f);
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);

        k1Vec = k1;
        k2Vec = k2;
        k3Vec = k3;
    }

    public float Update(float T, float x, float? xd = null)
    {
        float velocity = xd.HasValue ? xd.Value : (x - xp) / T;
        xp = x;

        // Integrate position by velocity
        y = y + T * yd;

        // Integrate velocity by acceleration
        yd = yd + T * ((x + k3 * velocity) - y - k1 * yd) / k2;

        return y;
    }

    public Vector3 Update(float T, Vector3 x, Vector3? xd = null)
    {
        Vector3 velocity = xd.HasValue ? xd.Value : (x - xpVec) / T;
        xpVec = x;

        // Integrate position by velocity
        yVec = yVec + T * ydVec;

        // Integrate velocity by acceleration
        ydVec = ydVec + T * ((x + k3Vec * velocity) - yVec - k1Vec * ydVec) / k2Vec;

        return yVec;
    }
}
