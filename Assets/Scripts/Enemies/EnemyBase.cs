using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public virtual void Hit()
    {
        Debug.Log("Enemy hit!");
        // This method can be overridden by derived classes to add specific behavior on hit
    }
}
