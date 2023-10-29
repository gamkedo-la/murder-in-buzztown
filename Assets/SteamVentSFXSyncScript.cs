using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVentSFXSyncScript : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public AudioSource audioSource;

    public int particleThreshold = 15; // You can adjust this threshold based on your particle burst size

    private int previousParticleCount = 0;

    private void Awake()
    {
        particleSystem = gameObject.transform.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Get the current particle count
        int currentParticleCount = particleSystem.particleCount;

        // Check for a significant increase in particle count compared to the previous frame
        if ((currentParticleCount - previousParticleCount) > particleThreshold)
        {
            // Play the sound effect
            audioSource.Play();
        }

        // Update the previous particle count for the next frame
        previousParticleCount = currentParticleCount;
    }
}
