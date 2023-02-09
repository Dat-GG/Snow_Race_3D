using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleToTarget : MonoBehaviour
{
    public Transform Target;
    public int Number;
    public float Delay;
    public float Speed;

    private ParticleSystem system;

    private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

    int count;
    // float delay;
    float _delay;
    void Play()
    {
        if (system == null)
            system = GetComponent<ParticleSystem>();



        if (system == null)
        {
            this.enabled = false;
        }
        else
        {
            _delay = Delay;
            system.maxParticles = Number;
            system.Play();

        }
    }

    private void OnEnable()
    {
        Play();
    }
    void Update()
    {
        _delay -= Time.deltaTime;
        if (_delay > 0) return;
        count = system.GetParticles(particles);

        int _inTarget = 0;

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = system.transform.TransformPoint(particle.position);
            Vector3 v2 = Target.transform.position;

            //パーティクル生成残り時間に応じて距離をつめる
            Vector3 tarPosi = (v2 - v1) * Speed * (particle.remainingLifetime / particle.startLifetime);
            //particle.position = system.transform.InverseTransformPoint(v2 - tarPosi);

            particle.position = Vector3.Lerp(particle.position, v2, Speed);
            if (Vector3.Distance(v1, v2) < 0.1f)
                _inTarget++;
            particles[i] = particle;
        }

        system.SetParticles(particles, count);
        if (_inTarget == Number)
            gameObject.SetActive(false);
    }
}
