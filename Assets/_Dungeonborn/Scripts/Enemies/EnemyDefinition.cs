using UnityEngine;

namespace Dungeonborn.Enemies
{
    public enum EnemyAttackStyle
    {
        Melee,
        Ranged,
        HeavyMelee
    }

    [CreateAssetMenu(menuName = "Dungeonborn/Enemies/Enemy Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        [SerializeField] private string enemyId = "enemy";
        [SerializeField] private string displayName = "Enemy";
        [SerializeField] private EnemyAttackStyle attackStyle = EnemyAttackStyle.Melee;
        [SerializeField] private float maxHealth = 30f;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float damage = 8f;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private float attackCooldown = 1.4f;
        [SerializeField] private Color bodyColor = Color.gray;

        public string EnemyId => enemyId;
        public string DisplayName => displayName;
        public EnemyAttackStyle AttackStyle => attackStyle;
        public float MaxHealth => maxHealth;
        public float MoveSpeed => moveSpeed;
        public float Damage => damage;
        public float AttackRange => attackRange;
        public float AttackCooldown => attackCooldown;
        public Color BodyColor => bodyColor;
    }
}
