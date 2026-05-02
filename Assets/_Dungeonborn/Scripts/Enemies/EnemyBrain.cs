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

            if (definition.AttackStyle == EnemyAttackStyle.Ranged && projectilePrefab != null)
            {
                var projectile = Instantiate(projectilePrefab, origin.position + direction, Quaternion.LookRotation(direction));
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
    }
}
