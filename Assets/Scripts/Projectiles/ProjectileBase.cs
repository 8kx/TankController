using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.Hit();
            OnHitEnemy(enemy);
        }
        Destroy(gameObject);
    }

    protected virtual void OnHitEnemy(EnemyBase enemy)
    {
        // This method can be overridden by derived classes to add specific behavior on hit
    }
}
