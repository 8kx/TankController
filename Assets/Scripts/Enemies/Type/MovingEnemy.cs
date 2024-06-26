using UnityEngine;
using System.Collections.Generic;

public class MovingEnemy : EnemyBase
{
    public Transform player;
    public float moveSpeed = 2f;
    public float obstacleAvoidanceDistance = 1.0f;
    public LayerMask obstacleLayer;
    private Pathfinder pathfinder;
    private GridManager gridManager;
    private List<Vector2> path;
    private int currentWaypointIndex = 0;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = new Pathfinder(gridManager);
    }

    void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(player.position.x, player.position.y);

        if (path == null || path.Count == 0 || currentWaypointIndex >= path.Count)
        {
            path = pathfinder.FindPath(currentPosition, targetPosition);
            currentWaypointIndex = 0;
        }

        if (path != null && currentWaypointIndex < path.Count)
        {
            Vector2 nextWaypoint = path[currentWaypointIndex];
            if (Vector2.Distance(currentPosition, nextWaypoint) < 0.1f)
            {
                currentWaypointIndex++;
            }
            else
            {
                MoveTowards(nextWaypoint);
            }
        }
    }

    void MoveTowards(Vector2 target)
    {
        Vector3 direction = new Vector3(target.x, transform.position.y, target.y) - transform.position;

        // Perform raycasting for obstacle avoidance
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, obstacleAvoidanceDistance, obstacleLayer))
        {
            Vector3 avoidanceDirection = Vector3.Cross(hit.normal, Vector3.up);
            direction = avoidanceDirection;
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    public override void Hit()
    {
        // Implement what happens when the enemy is hit
        Destroy(gameObject);
    }
}
