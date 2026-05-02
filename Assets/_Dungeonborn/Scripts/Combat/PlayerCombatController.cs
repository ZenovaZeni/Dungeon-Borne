using Dungeonborn.Characters;
using Dungeonborn.Enemies;
using Dungeonborn.Input;
using Dungeonborn.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonborn.Combat
{
    [RequireComponent(typeof(PlayerInputReader))]
    [RequireComponent(typeof(PlayerMotor))]
    public sealed class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private SkillDefinition basicAttack;
        [SerializeField] private SkillDefinition cleave;
        [SerializeField] private SkillDefinition stomp;
        [SerializeField] private SkillDefinition rageUltimate;
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private LayerMask enemyLayers;
        [SerializeField] private ShockwaveProjectile shockwavePrefab;
        [SerializeField] private DamageNumberSpawner damageNumbers;
        [SerializeField] private float basicKnockback = 0.28f;
        [SerializeField] private float cleaveKnockback = 0.45f;
        [SerializeField] private float stompKnockback = 0.65f;
        [SerializeField] private float rageKnockback = 0.9f;
        [SerializeField] private float prototypeEnemyHitTolerance = 0.9f;

        private readonly SkillCooldownBook cooldowns = new SkillCooldownBook();
        private PlayerInputReader input;
        private PlayerMotor motor;
        private PlayerLegendaryModifiers legendaryModifiers;

        public SkillCooldownBook Cooldowns => cooldowns;

        private void Awake()
        {
            input = GetComponent<PlayerInputReader>();
            motor = GetComponent<PlayerMotor>();
            legendaryModifiers = GetComponent<PlayerLegendaryModifiers>();
            if (attackOrigin == null)
            {
                attackOrigin = transform;
            }
        }

        private void OnEnable()
        {
            input.BasicAttackPressed += UseBasicAttack;
            input.Skill1Pressed += UseCleave;
            input.Skill2Pressed += UseStomp;
            input.Skill3Pressed += UseRagePlaceholder;
            input.UltimatePressed += UseRagePlaceholder;
        }

        private void OnDisable()
        {
            input.BasicAttackPressed -= UseBasicAttack;
            input.Skill1Pressed -= UseCleave;
            input.Skill2Pressed -= UseStomp;
            input.Skill3Pressed -= UseRagePlaceholder;
            input.UltimatePressed -= UseRagePlaceholder;
        }

        private void Update()
        {
            cooldowns.Tick(Time.deltaTime);
        }

        private void UseBasicAttack()
        {
            UseAreaSkill(basicAttack);
        }

        private void UseCleave()
        {
            if (!UseAreaSkill(cleave))
            {
                return;
            }

            if (legendaryModifiers != null &&
                legendaryModifiers.Has(LegendaryModifier.EchoAxe))
            {
                var origin = attackOrigin != null ? attackOrigin : transform;
                var projectile = shockwavePrefab != null
                    ? Instantiate(shockwavePrefab, origin.position + motor.FacingDirection, Quaternion.LookRotation(motor.FacingDirection))
                    : CreateFallbackShockwave(origin.position + motor.FacingDirection, motor.FacingDirection);

                projectile.Launch(motor.FacingDirection, cleave.Damage * 0.7f, enemyLayers, damageNumbers);
            }
        }

        private void UseStomp()
        {
            UseAreaSkill(stomp);
        }

        private void UseRagePlaceholder()
        {
            // Prototype 0.1 placeholder: proves an ultimate button/cooldown path without adding a rage resource yet.
            UseAreaSkill(rageUltimate);
        }

        private bool UseAreaSkill(SkillDefinition skill)
        {
            if (skill == null || !cooldowns.TryStart(skill))
            {
                return false;
            }

            var origin = attackOrigin != null ? attackOrigin : transform;
            SpawnPlaytestAttackMarker(skill, origin);
            var damagedTargets = new HashSet<Damageable>();
            foreach (var target in GetSkillTargets(skill, origin.position))
            {
                if (target == null || !damagedTargets.Add(target))
                {
                    continue;
                }

                var toTarget = target.transform.position - origin.position;
                toTarget.y = 0f;
                var result = target.ApplyDamage(skill.Damage);
                target.ApplyKnockback(toTarget.sqrMagnitude > 0.001f ? toTarget : motor.FacingDirection, GetKnockbackFor(skill));
                if (damageNumbers != null)
                {
                    damageNumbers.Spawn(target.transform.position + Vector3.up * 1.8f, result.Amount);
                }
            }

            return true;
        }

        private IEnumerable<Damageable> GetSkillTargets(SkillDefinition skill, Vector3 origin)
        {
            foreach (var hit in Physics.OverlapSphere(origin, skill.Range, enemyLayers, QueryTriggerInteraction.Collide))
            {
                var damageable = hit.GetComponentInParent<Damageable>();
                if (IsValidSkillTarget(damageable, skill, origin))
                {
                    yield return damageable;
                }
            }

            // CharacterController-backed prototype enemies can be unreliable with simple overlap checks.
            // This keeps the sandbox testable while the art/animation collision setup is still placeholder.
            foreach (var damageable in FindObjectsByType<Damageable>(FindObjectsInactive.Exclude))
            {
                if (IsValidSkillTarget(damageable, skill, origin))
                {
                    yield return damageable;
                }
            }
        }

        private bool IsValidSkillTarget(Damageable damageable, SkillDefinition skill, Vector3 origin)
        {
            if (damageable == null || damageable.IsDead || damageable.gameObject == gameObject)
            {
                return false;
            }

            var isEnemyLayer = (enemyLayers.value & (1 << damageable.gameObject.layer)) != 0;
            var hasEnemyBrain = damageable.TryGetComponent<EnemyBrain>(out _);
            if (!isEnemyLayer && !hasEnemyBrain)
            {
                return false;
            }

            var toTarget = damageable.transform.position - origin;
            toTarget.y = 0f;
            var effectiveRange = skill.Range + prototypeEnemyHitTolerance;
            if (toTarget.sqrMagnitude > effectiveRange * effectiveRange)
            {
                return false;
            }

            if (skill.Shape != SkillShape.Cone)
            {
                return true;
            }

            if (toTarget.sqrMagnitude <= 0.001f)
            {
                return true;
            }

            var angle = Vector3.Angle(motor.FacingDirection, toTarget.normalized);
            return angle <= skill.ConeAngle * 0.5f;
        }

        private void SpawnPlaytestAttackMarker(SkillDefinition skill, Transform origin)
        {
            // Prototype 0.1 playtest feedback: temporary primitive until real animations/VFX exist.
            var marker = GameObject.CreatePrimitive(skill.Shape == SkillShape.Circle ? PrimitiveType.Cylinder : PrimitiveType.Cube);
            marker.name = $"Playtest_{skill.DisplayName}_Marker";
            Destroy(marker.GetComponent<Collider>());

            var markerRenderer = marker.GetComponent<Renderer>();
            markerRenderer.material.color = GetMarkerColor(skill);

            if (skill.Shape == SkillShape.Circle)
            {
                marker.transform.position = origin.position + Vector3.up * 0.04f;
                var scale = skill.Slot == AbilitySlot.Ultimate ? skill.Range * 2.35f : skill.Range * 2f;
                marker.transform.localScale = new Vector3(scale, 0.05f, scale);
            }
            else
            {
                marker.transform.position = origin.position + motor.FacingDirection * (skill.Range * 0.5f) + Vector3.up * 0.08f;
                marker.transform.rotation = Quaternion.LookRotation(motor.FacingDirection, Vector3.up);
                var width = skill.Slot == AbilitySlot.BasicAttack ? skill.Radius * 1.25f : skill.Radius * 2.2f;
                marker.transform.localScale = new Vector3(width, 0.07f, skill.Range);
            }

            Destroy(marker, skill.Slot == AbilitySlot.Ultimate ? 0.32f : 0.2f);
        }

        private float GetKnockbackFor(SkillDefinition skill)
        {
            return skill.Slot switch
            {
                AbilitySlot.BasicAttack => basicKnockback,
                AbilitySlot.Skill1 => cleaveKnockback,
                AbilitySlot.Skill2 => stompKnockback,
                AbilitySlot.Ultimate => rageKnockback,
                _ => 0.25f
            };
        }

        private static Color GetMarkerColor(SkillDefinition skill)
        {
            return skill.Slot switch
            {
                AbilitySlot.BasicAttack => Color.white,
                AbilitySlot.Skill1 => new Color(0.15f, 0.9f, 1f),
                AbilitySlot.Skill2 => new Color(1f, 0.65f, 0.1f),
                AbilitySlot.Ultimate => new Color(1f, 0.08f, 0.04f),
                _ => skill.DebugColor
            };
        }

        private static ShockwaveProjectile CreateFallbackShockwave(Vector3 position, Vector3 direction)
        {
            var shockwaveObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shockwaveObject.name = "FallbackEchoAxeShockwave";
            shockwaveObject.transform.position = position;
            shockwaveObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            shockwaveObject.transform.localScale = new Vector3(1.2f, 0.2f, 0.6f);
            Destroy(shockwaveObject.GetComponent<Collider>());
            shockwaveObject.GetComponent<Renderer>().material.color = new Color(0.35f, 0.9f, 1f);
            return shockwaveObject.AddComponent<ShockwaveProjectile>();
        }
    }
}
