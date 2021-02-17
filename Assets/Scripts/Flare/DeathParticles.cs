using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DeathParticles : MonoBehaviour
{
    [SerializeField] private float aliveTime;
    private ParticleSystem particle;
    private ParticleSystem.MainModule definitions;

    private void Awake() 
    {
        particle = GetComponent<ParticleSystem>();
        definitions = particle.main;
    }

    public void SetStartSize(float size)
    {
        definitions.startSizeMultiplier = size;
    }

    private void Start() 
    {
        Invoke("DestroyObject", aliveTime);    
    }

    public void Play()
    {
        particle.Play(true);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
