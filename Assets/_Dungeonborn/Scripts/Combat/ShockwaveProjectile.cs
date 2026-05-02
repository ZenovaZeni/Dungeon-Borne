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
        [SerializeField] private float knockback = 0.5f;
        [SerializeField] private float trailInterval = 0.06f;

        private readonly HashSet<Damageable> hitTargets = new HashSet<Damageable>();
        private Vector3 direction;
        private float damage;
        private LayerMask targetLayers;
        private DamageNumberSpawner damageNumbers;
        private float remainingLifetime;
        private float trailTimeRemaining;

        public void Launch(Vector3 launchDirection, float launchDamage, LayerMask layers, DamageNumberSpawner numberSpawner)
        {
            direction = launchDirection.normalized;
            damage = launchDamage;
            targetLayers = layers;
            damageNumbers = numberSpawner;
            remainingLifetime = lifetime;
            trailTimeRemaining = 0f;
        }

        private void Update()
        {
            transform.position += direction * (speed * Time.deltaTime);
            remainingLifetime -= Time.deltaTime;
            trailTimeRemaining -= Time.deltaTime;
            if (trailTimeRemaining <= 0f)
            {
                SpawnTravelFlash();
                trailTimeRemaining = trailInterval;
            }

            foreach (var hit in Physics.OverlapSphere(transform.position, radius, targetLayers))
            {
                if (hit.TryGetComponent<Damageable>(out var damageable) && !damageable.IsDead && hitTargets.Add(damageable))
                {
                    var result = damageable.ApplyDamage(damage);
                    damageable.ApplyKnockback(direction, knockback);
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

        private void SpawnTravelFlash()
        {
            var flash = GameObject.CreatePrimitive(PrimitiveType.Cube);
            flash.name = "EchoAxeShockwaveTrail";
            flash.transform.position = transform.position;
            flash.transform.rotation = transform.rotation;
            flash.transform.localScale = transform.localScale * 0.85f;
            Destroy(flash.GetComponent<Collider>());
            flash.GetComponent<Renderer>().material.color = new Color(0.35f, 1f, 1f);
            Destroy(flash, 0.12f);
        }
    }
}
