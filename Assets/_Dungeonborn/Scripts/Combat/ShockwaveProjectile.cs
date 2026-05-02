using System.Collections.Generic;
using Dungeonborn.Characters;
using Dungeonborn.UI;
using UnityEngine;

namespace Dungeonborn.Combat
{
    public sealed class ShockwaveProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 9f;
        [SerializeField] private float lifetime = 0.75f;
        [SerializeField] private float radius = 0.8f;

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

            foreach (var hit in Physics.OverlapSphere(transform.position, radius, targetLayers))
            {
                if (hit.TryGetComponent<Damageable>(out var damageable) && !damageable.IsDead && hitTargets.Add(damageable))
                {
                    var result = damageable.ApplyDamage(damage);
                    if (damageNumbers != null)
                    {
                        damageNumbers.Spawn(hit.transform.position + Vector3.up * 1.8f, result.Amount);
                    }
                }
            }

            if (remainingLifetime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
