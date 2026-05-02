using Dungeonborn.Characters;
using Dungeonborn.Input;
using Dungeonborn.UI;
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
            var hits = Physics.OverlapSphere(origin.position, skill.Range, enemyLayers);
            foreach (var hit in hits)
            {
                var toTarget = hit.transform.position - origin.position;
                toTarget.y = 0f;

                if (skill.Shape == SkillShape.Cone)
                {
                    var angle = Vector3.Angle(motor.FacingDirection, toTarget.normalized);
                    if (angle > skill.ConeAngle * 0.5f)
                    {
                        continue;
                    }
                }

                if (hit.TryGetComponent<Damageable>(out var damageable))
                {
                    var result = damageable.ApplyDamage(skill.Damage);
                    if (damageNumbers != null)
                    {
                        damageNumbers.Spawn(hit.transform.position + Vector3.up * 1.8f, result.Amount);
                    }
                }
            }

            return true;
        }

        private void SpawnPlaytestAttackMarker(SkillDefinition skill, Transform origin)
        {
            // Prototype 0.1 playtest feedback: temporary primitive until real animations/VFX exist.
            var marker = GameObject.CreatePrimitive(skill.Shape == SkillShape.Circle ? PrimitiveType.Cylinder : PrimitiveType.Cube);
            marker.name = $"Playtest_{skill.DisplayName}_Marker";
            Destroy(marker.GetComponent<Collider>());

            var markerRenderer = marker.GetComponent<Renderer>();
            markerRenderer.material.color = new Color(skill.DebugColor.r, skill.DebugColor.g, skill.DebugColor.b, 0.45f);

            if (skill.Shape == SkillShape.Circle)
            {
                marker.transform.position = origin.position + Vector3.up * 0.04f;
                marker.transform.localScale = new Vector3(skill.Range * 2f, 0.04f, skill.Range * 2f);
            }
            else
            {
                marker.transform.position = origin.position + motor.FacingDirection * (skill.Range * 0.5f) + Vector3.up * 0.08f;
                marker.transform.rotation = Quaternion.LookRotation(motor.FacingDirection, Vector3.up);
                marker.transform.localScale = new Vector3(skill.Radius * 1.4f, 0.06f, skill.Range);
            }

            Destroy(marker, 0.16f);
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
