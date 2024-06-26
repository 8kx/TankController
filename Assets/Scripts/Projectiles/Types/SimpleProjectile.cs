using UnityEngine;

public class SimpleProjectile : ProjectileBase
{
    public float speed = 20f;
    public float lifeTime = 5f;

    private void Start()
    {
        // Destroy the projectile after a certain time to prevent memory leaks
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected override void OnHitEnemy(EnemyBase enemy)
    {
        base.OnHitEnemy(enemy);
        // Additional behavior for SimpleProjectile when hitting an enemy
    }
}
