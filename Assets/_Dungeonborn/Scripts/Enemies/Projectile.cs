using System.Collections.Generic;
using Dungeonborn.Characters;
using Dungeonborn.UI;
using UnityEngine;

namespace Dungeonborn.Enemies
{
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 7f;
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private float hitRadius = 0.45f;

        private readonly HashSet<Damageable> hitTargets = new HashSet<Damageable>();
        private Vector3 direction;
        private float damage;
        private LayerMask targetLayers;
        private DamageNumberSpawner damageNumbers;
        private float remainingLifetime;

        public void Launch(Vector3 launchDirection, float launchDamage, LayerMask layers, DamageNumberSpawner numberSpawner)
        {
            direction = launchDirection.normalized;
            damage = launchDamage;
            targetLayers = layers;
            damageNumbers = numberSpawner;
            remainingLifetime = lifetime;
        }

        private void Update()
        {
            transform.position += direction * (speed * Time.deltaTime);
            remainingLifetime -= Time.deltaTime;

            foreach (var hit in Physics.OverlapSphere(transform.position, hitRadius, targetLayers))
            {
                if (hit.TryGetComponent<Damageable>(out var damageable) && hitTargets.Add(damageable))
                {
                    var result = damageable.ApplyDamage(damage);
                    damageNumbers?.Spawn(hit.transform.position + Vector3.up * 1.8f, result.Amount);
                    Destroy(gameObject);
                    return;
                }
            }

            if (remainingLifetime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
