using UnityEngine;

public class TankEngineFXController : MonoBehaviour
{
    public TankMovement tankMovement; // Reference to the tank movement script
    public ParticleSystem[] smokeParticleSystems; // Array of smoke particle systems
    public float idleEmissionRate = 5f; // Emission rate when idling
    public float moveEmissionRate = 50f; // Emission rate when moving
    public float idleSpeed = 0.5f; // Particle speed when idling
    public float moveSpeed = 2f; // Particle speed when moving
    public float idleLifetime = 2f; // Particle lifetime when idling
    public float moveLifetime = 1f; // Particle lifetime when moving
    public float idleSize = 1f; // Particle size when idling
    public float moveSize = 2f; // Particle size when moving

    private ParticleSystem.EmissionModule[] emissionModules;
    private ParticleSystem.MainModule[] mainModules;

    private void Start()
    {
        emissionModules = new ParticleSystem.EmissionModule[smokeParticleSystems.Length];
        mainModules = new ParticleSystem.MainModule[smokeParticleSystems.Length];

        for (int i = 0; i < smokeParticleSystems.Length; i++)
        {
            emissionModules[i] = smokeParticleSystems[i].emission;
            mainModules[i] = smokeParticleSystems[i].main;
        }
    }

    private void Update()
    {
        float speed = Mathf.Abs(tankMovement.currentSpeed);

        for (int i = 0; i < smokeParticleSystems.Length; i++)
        {
            if (speed > 0)
            {
                // Tank is moving
                emissionModules[i].rateOverTime = moveEmissionRate;
                mainModules[i].startSpeed = moveSpeed;
                mainModules[i].startLifetime = moveLifetime;
                mainModules[i].startSize = moveSize;
            }
            else
            {
                // Tank is idling
                emissionModules[i].rateOverTime = idleEmissionRate;
                mainModules[i].startSpeed = idleSpeed;
                mainModules[i].startLifetime = idleLifetime;
                mainModules[i].startSize = idleSize;
            }
        }
    }
}
