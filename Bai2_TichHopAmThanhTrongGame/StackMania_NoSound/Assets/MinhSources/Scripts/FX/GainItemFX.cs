using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class GainItemFX : MonoBehaviour
{
    public Transform target;
    public Transform appliedScaleObject;
    bool isScaling = false;

    ParticleSystem _particleSystem;
    public float delay = 1f;
    public float targetSize = 0.15f;

    //
    // Start is called before the first frame update
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_particleSystem.IsAlive())
        {
            UpdatePosition();
        }
    }

    private void OnDestroy()
    {

    }

    private void OnParticleTrigger()
    {
        //
        if (isScaling || appliedScaleObject == null) return;
        isScaling = true;

        appliedScaleObject.DOScale(Vector3.one * 1.1f, 0.025f).OnComplete(() =>
        {
            appliedScaleObject.DOScale(Vector3.one, 0.025f).OnComplete(() =>
            {
                isScaling = false;
            });
        });
    }

    public void UpdatePosition()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_particleSystem.particleCount];
        int numParticlesAlive = _particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            float lifeTime = particles[i].startLifetime - particles[i].remainingLifetime;

            if (lifeTime < delay) {
                continue;
            };
            // Modify the position of each particle
            particles[i].velocity = Vector3.zero;

            float moveTimer = lifeTime - delay;

            if (moveTimer > particles[i].startLifetime - delay)
            {
                continue;
            }
            particles[i].position = Vector3.Lerp(particles[i].position, target.position, moveTimer / 1.5f);
            particles[i].startSize = Mathf.Lerp(particles[i].startSize, targetSize, moveTimer / 1.5f);
        }

        _particleSystem.SetParticles(particles, numParticlesAlive);
    }
}
