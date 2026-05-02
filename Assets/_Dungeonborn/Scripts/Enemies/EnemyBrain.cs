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
        private CharacterController controller;
        private CooldownTimer attackCooldown;
        private bool attackWindupActive;
        private float attackWindupRemaining;
        private Vector3 pendingAttackDirection;

        private void Awake()
        {
            damageable = GetComponent<Damageable>();
            controller = GetComponent<CharacterController>();
            if (controller == null)
            {
                controller = gameObject.AddComponent<CharacterController>();
            }

            controller.radius = 0.45f;
            controller.height = 2f;
            controller.center = Vector3.zero;
            DisablePrimitiveCollider();

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

            if (definition.AttackStyle == EnemyAttackStyle.Ranged)
            {
                MoveRanged(distance, toTarget);
            }
            else if (distance > definition.AttackRange)
            {
                Move(toTarget.normalized * (definition.MoveSpeed * Time.deltaTime));
            }

            if (toTarget.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(toTarget.normalized);
            }

            if (attackWindupActive)
            {
                attackWindupRemaining -= Time.deltaTime;
                if (attackWindupRemaining <= 0f)
                {
                    attackWindupActive = false;
                    Attack(pendingAttackDirection);
                }

                return;
            }

            if (distance <= definition.AttackRange && attackCooldown.TryStart())
            {
                StartAttackWindup(toTarget.normalized);
            }
        }

        private void MoveRanged(float distance, Vector3 toTarget)
        {
            if (toTarget.sqrMagnitude <= 0.001f)
            {
                return;
            }

            var direction = toTarget.normalized;
            var preferredRange = definition.AttackRange * 0.68f;
            var tooCloseRange = definition.AttackRange * 0.45f;

            if (distance > definition.AttackRange)
            {
                Move(direction * (definition.MoveSpeed * Time.deltaTime));
            }
            else if (distance < tooCloseRange)
            {
                Move(-direction * (definition.MoveSpeed * Time.deltaTime));
            }
            else if (distance < preferredRange)
            {
                Move(-direction * (definition.MoveSpeed * 0.45f * Time.deltaTime));
            }
        }

        private void Move(Vector3 displacement)
        {
            displacement.y = 0f;
            if (controller != null && controller.enabled)
            {
                controller.Move(displacement);
                return;
            }

            transform.position += displacement;
        }

        private void DisablePrimitiveCollider()
        {
            foreach (var attachedCollider in GetComponents<Collider>())
            {
                if (attachedCollider is CharacterController)
                {
                    continue;
                }

                attachedCollider.enabled = false;
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

        private void StartAttackWindup(Vector3 direction)
        {
            pendingAttackDirection = direction.sqrMagnitude > 0.001f ? direction.normalized : transform.forward;
            attackWindupRemaining = GetWindupDuration();
            attackWindupActive = true;
            SpawnTelegraphMarker(attackOrigin != null ? attackOrigin : transform, pendingAttackDirection, attackWindupRemaining);
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
                    targetDamageable.ApplyKnockback(direction, definition.AttackStyle == EnemyAttackStyle.HeavyMelee ? 0.45f : 0.22f);
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

        private void SpawnTelegraphMarker(Transform origin, Vector3 direction, float duration)
        {
            var marker = GameObject.CreatePrimitive(definition.AttackStyle == EnemyAttackStyle.Ranged ? PrimitiveType.Cube : PrimitiveType.Cylinder);
            marker.name = $"Telegraph_{definition.DisplayName}";
            Destroy(marker.GetComponent<Collider>());

            var markerRenderer = marker.GetComponent<Renderer>();
            markerRenderer.material.color = definition.AttackStyle switch
            {
                EnemyAttackStyle.Ranged => new Color(1f, 0.08f, 0.08f),
                EnemyAttackStyle.HeavyMelee => new Color(1f, 0.18f, 0.02f),
                _ => new Color(1f, 0.85f, 0.1f)
            };

            if (definition.AttackStyle == EnemyAttackStyle.Ranged)
            {
                marker.transform.position = origin.position + direction.normalized * (definition.AttackRange * 0.5f) + Vector3.up * 0.04f;
                marker.transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                marker.transform.localScale = new Vector3(0.32f, 0.05f, definition.AttackRange);
            }
            else
            {
                var scale = definition.AttackRange * (definition.AttackStyle == EnemyAttackStyle.HeavyMelee ? 2.5f : 2f);
                marker.transform.position = origin.position + Vector3.up * 0.04f;
                marker.transform.localScale = new Vector3(scale, 0.05f, scale);
            }

            Destroy(marker, duration);
        }

        private float GetWindupDuration()
        {
            return definition.AttackStyle switch
            {
                EnemyAttackStyle.Ranged => 0.3f,
                EnemyAttackStyle.HeavyMelee => 0.45f,
                _ => 0.2f
            };
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
