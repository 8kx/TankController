using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform tankTransform; // Reference to the tank's transform
    public float spawnInterval = 2f;
    public float minSpawnDistance = 5f; // Minimum distance from the tank to spawn a target
    public float spawnAreaSize = 20f; // Size of the area where targets can spawn
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnTarget();
            timer = 0f;
        }
    }

    private void SpawnTarget()
    {
        Vector3 spawnPosition;
        do
        {
            spawnPosition = new Vector3(
                Random.Range(-spawnAreaSize, spawnAreaSize),
                0,
                Random.Range(-spawnAreaSize, spawnAreaSize)
            );
        } while (Vector3.Distance(spawnPosition, tankTransform.position) < minSpawnDistance);

        Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
    }
}
