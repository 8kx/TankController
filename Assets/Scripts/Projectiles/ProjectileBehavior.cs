using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
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

    private void OnCollisionEnter(Collision collision)
    {
        // Add collision handling logic here (e.g., apply damage, effects)
        
        // Destroy the projectile on collision
        Destroy(gameObject);
    }
}
