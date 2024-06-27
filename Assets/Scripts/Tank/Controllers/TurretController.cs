using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    public Transform tankTransform; // Reference to the tank's transform
    public Transform barrelTransform; // Reference to the barrel's transform
    public TankBodyAnimationController bodyAnimationController; // Reference to the body animation controller
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Reference to the fire point

    [Header("Second Order Dynamics Parameters")]
    public float frequency = 1.0f;
    public float damping = 0.5f;
    public float response = 1.0f;
    public float angleMulti = 1.0f; // Multiplier to speed up the rotation
    public float shotForce = -1.0f;

    private TankControls controls;
    private CannonFXController cannonFXController;
    private Vector2 rawAimingInput;
    private SecondOrderDynamics dynamics;
    private Vector3 lastAimDirection;

    private void Awake()
    {
        controls = new TankControls();
        
        cannonFXController = GetComponent<CannonFXController>();

        // Initialize SecondOrderDynamics with exposed parameters
        dynamics = new SecondOrderDynamics(frequency, damping, response, Vector3.zero);

        // Register the callback methods for the actions
        controls.Tank.Aim.performed += ctx => rawAimingInput = ctx.ReadValue<Vector2>();
        controls.Tank.Aim.canceled += ctx => rawAimingInput = Vector2.zero;
        controls.Tank.Fire.started += ctx => Fire();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        // Initialize the last aim direction to be forward
        lastAimDirection = transform.forward;
    }

    private void Update()
    {
        // Update Controls input
        rawAimingInput = controls.Tank.Aim.ReadValue<Vector2>();

        // Update SecondOrderDynamics parameters at runtime
        dynamics.UpdateParameters(frequency, damping, response);

        if (rawAimingInput != Vector2.zero)
        {
            HandleAiming();
        }
        else
        {
            MaintainLastAimedDirection();
        }

        // Constrain the turret's position to the tank's position
        if (tankTransform != null)
        {
            transform.position = tankTransform.position;
        }

        // Keep the barrel parallel to the ground
        MaintainBarrelLevel();
    }

    private void HandleAiming()
    {
        Vector3 direction = new Vector3(rawAimingInput.x, 0, rawAimingInput.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            lastAimDirection = direction;
            RotateTurret(direction);
        }
    }

    private void MaintainLastAimedDirection()
    {
        RotateTurret(lastAimDirection);
    }

    private void RotateTurret(Vector3 direction)
    {
        // Convert the direction to local space relative to the parent
        Vector3 localDirection = tankTransform.InverseTransformDirection(direction);
        float targetAngle = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;
        float currentAngle = transform.localEulerAngles.y;
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        // Speed up the rotation
        angleDifference *= angleMulti;

        // Use SecondOrderDynamics to smooth angular acceleration
        float angularAcceleration = dynamics.Update(Time.deltaTime, angleDifference);

        // Apply the smoothed angular acceleration to the turret's rotation
        float smoothedAngle = currentAngle + angularAcceleration * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, smoothedAngle, 0);
    }

    private void MaintainBarrelLevel()
    {
        if (barrelTransform != null)
        {
            // Set the barrel's x and z rotations to zero in world space
            Vector3 worldEulerAngles = barrelTransform.eulerAngles;
            worldEulerAngles.x = 0f;
            worldEulerAngles.z = 0f;
            barrelTransform.eulerAngles = worldEulerAngles;
        }
    }

    private void Fire()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Instantiate the projectile at the fire point
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Add external force to the body animation controller
            bodyAnimationController.AddExternalForce(firePoint.forward * shotForce);

            // Play the muzzle flash particle effect
            if (cannonFXController != null)
            {
                cannonFXController.Play();
            }
        }
    }
}
