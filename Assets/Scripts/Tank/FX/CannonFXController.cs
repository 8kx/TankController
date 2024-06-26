using UnityEngine;

public class CannonFXController : MonoBehaviour
{
    public ParticleSystem smokeEffect; // Particle system for smoke
    public ParticleSystem muzzleFlashEffect; // Particle system for muzzle flash
    public ParticleSystem groundEffect; // Particle system for ground effect
    public AudioSource firingSound; // Audio source for firing sound

    // Public method to play the cannon effects
    public void Play()
    {
        // Play smoke effect
        if (smokeEffect != null)
        {
            smokeEffect.Play();
        }

        // Play muzzle flash effect
        if (muzzleFlashEffect != null)
        {
            muzzleFlashEffect.Play();
        }

        // Play ground effect
        if (groundEffect != null)
        {
            groundEffect.Play();
        }

        // Play firing sound
        if (firingSound != null)
        {
            firingSound.Play();
        }
    }
}
