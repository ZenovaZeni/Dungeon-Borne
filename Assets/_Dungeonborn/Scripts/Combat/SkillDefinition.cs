using UnityEngine;

namespace Dungeonborn.Combat
{
    public enum SkillShape
    {
        Cone,
        Circle,
        Projectile
    }

    [CreateAssetMenu(menuName = "Dungeonborn/Combat/Skill Definition")]
    public sealed class SkillDefinition : ScriptableObject
    {
        [SerializeField] private string skillId = "skill";
        [SerializeField] private string displayName = "Skill";
        [SerializeField] private AbilitySlot slot = AbilitySlot.BasicAttack;
        [SerializeField] private SkillShape shape = SkillShape.Cone;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float cooldown = 1f;
        [SerializeField] private float range = 2f;
        [SerializeField] private float radius = 1f;
        [SerializeField] private float coneAngle = 80f;
        [SerializeField] private float force = 0f;
        [SerializeField] private Color debugColor = Color.white;

        public string SkillId => skillId;
        public string DisplayName => displayName;
        public AbilitySlot Slot => slot;
        public SkillShape Shape => shape;
        public float Damage => damage;
        public float Cooldown => cooldown;
        public float Range => range;
        public float Radius => radius;
        public float ConeAngle => coneAngle;
        public float Force => force;
        public Color DebugColor => debugColor;
    }
}
