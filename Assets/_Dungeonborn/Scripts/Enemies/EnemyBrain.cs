using Dungeonborn.Characters;
using Dungeonborn.Combat;
using Dungeonborn.UI;
using UnityEngine;

namespace Dungeonborn.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public sealed class EnemyBrain : MonoBehaviour
    {
        [SerializeField] private EnemyDefinition definition;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private LayerMask playerLayers;
        [SerializeField] private DamageNumberSpawner damageNumbers;

        private Transform target;
        private Damageable damageable;
        private CooldownTimer attackCooldown;

        private void Awake()
        {
            damageable = GetComponent<Damageable>();
            if (attackOrigin == null)
            {
                attackOrigin = transform;
            }
        }

        private void Start()
        {
            Configure(definition);
            var player = GameObject.FindGameObjectWithTag("Player");
            target = player != null ? player.transform : null;
        }

        private void Update()
        {
            if (definition == null || target == null || damageable.IsDead)
            {
                return;
            }

            attackCooldown.Tick(Time.deltaTime);

            var toTarget = target.position - transform.position;
            toTarget.y = 0f;
            var distance = toTarget.magnitude;

            if (distance > definition.AttackRange)
            {
                transform.position += toTarget.normalized * (definition.MoveSpeed * Time.deltaTime);
            }

            if (toTarget.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(toTarget.normalized);
            }

            if (distance <= definition.AttackRange && attackCooldown.TryStart())
            {
                Attack(toTarget.normalized);
            }
        }

        public void Configure(EnemyDefinition configuredDefinition)
        {
            definition = configuredDefinition;
            if (definition == null)
            {
                return;
            }

            damageable.Configure(definition.MaxHealth);
            attackCooldown = new CooldownTimer(definition.AttackCooldown);
        }

        private void Attack(Vector3 direction)
        {
            var origin = attackOrigin != null ? attackOrigin : transform;
            SpawnPlaytestAttackMarker(origin, direction);

            if (definition.AttackStyle == EnemyAttackStyle.Ranged && projectilePrefab != null)
            {
                var projectile = Instantiate(projectilePrefab, origin.position + direction, Quaternion.LookRotation(direction));
                projectile.Launch(direction, definition.Damage, playerLayers, damageNumbers);
                return;
            }

            if (definition.AttackStyle == EnemyAttackStyle.Ranged)
            {
                var projectile = CreateFallbackProjectile(origin.position + direction, direction);
                projectile.Launch(direction, definition.Damage, playerLayers, damageNumbers);
                return;
            }

            foreach (var hit in Physics.OverlapSphere(origin.position, definition.AttackRange, playerLayers))
            {
                if (hit.TryGetComponent<Damageable>(out var targetDamageable))
                {
                    var result = targetDamageable.ApplyDamage(definition.Damage);
                    if (damageNumbers != null)
                    {
                        damageNumbers.Spawn(hit.transform.position + Vector3.up * 1.8f, result.Amount);
                    }
                }
            }
        }

        private void SpawnPlaytestAttackMarker(Transform origin, Vector3 direction)
        {
            // Prototype 0.1 playtest feedback: temporary enemy attack marker until telegraphs/VFX exist.
            var marker = GameObject.CreatePrimitive(definition.AttackStyle == EnemyAttackStyle.Ranged ? PrimitiveType.Cube : PrimitiveType.Cylinder);
            marker.name = $"Playtest_{definition.DisplayName}_AttackMarker";
            Destroy(marker.GetComponent<Collider>());

            var markerRenderer = marker.GetComponent<Renderer>();
            markerRenderer.material.color = definition.AttackStyle == EnemyAttackStyle.Ranged
                ? new Color(1f, 0.25f, 0.15f, 0.55f)
                : new Color(1f, 0.65f, 0.2f, 0.55f);

            if (definition.AttackStyle == EnemyAttackStyle.Ranged)
            {
                marker.transform.position = origin.position + direction.normalized * (definition.AttackRange * 0.5f) + Vector3.up * 0.1f;
                marker.transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                marker.transform.localScale = new Vector3(0.18f, 0.08f, definition.AttackRange);
            }
            else
            {
                marker.transform.position = origin.position + Vector3.up * 0.05f;
                marker.transform.localScale = new Vector3(definition.AttackRange * 2f, 0.05f, definition.AttackRange * 2f);
            }

            Destroy(marker, 0.18f);
        }

        private static Projectile CreateFallbackProjectile(Vector3 position, Vector3 direction)
        {
            var projectileObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectileObject.name = "FallbackArcherProjectile";
            projectileObject.transform.position = position;
            projectileObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            projectileObject.transform.localScale = Vector3.one * 0.35f;
            Destroy(projectileObject.GetComponent<Collider>());
            projectileObject.GetComponent<Renderer>().material.color = new Color(1f, 0.2f, 0.15f);
            return projectileObject.AddComponent<Projectile>();
        }
    }
}
