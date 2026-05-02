using Dungeonborn.Combat;
using Dungeonborn.Enemies;
using Dungeonborn.Loot;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Dungeonborn.Tests
{
    public sealed class PrototypeAssetReferenceTests
    {
        private const string Root = "Assets/_Dungeonborn";

        [Test]
        public void SkillDefinitionsHaveExpectedSlots()
        {
            AssertSkill("BasicAttack.asset", AbilitySlot.BasicAttack, SkillShape.Cone);
            AssertSkill("Cleave.asset", AbilitySlot.Skill1, SkillShape.Cone);
            AssertSkill("Stomp.asset", AbilitySlot.Skill2, SkillShape.Circle);
            AssertSkill("RagePlaceholder.asset", AbilitySlot.Ultimate, SkillShape.Circle);
        }

        [Test]
        public void EnemyDefinitionsHaveExpectedAttackStyles()
        {
            AssertEnemy("SkeletonGrunt.asset", EnemyAttackStyle.Melee);
            AssertEnemy("Archer.asset", EnemyAttackStyle.Ranged);
            AssertEnemy("Brute.asset", EnemyAttackStyle.HeavyMelee);
        }

        [Test]
        public void EchoAxeIsLegendaryCleaveModifier()
        {
            var item = AssetDatabase.LoadAssetAtPath<LootItemDefinition>(Root + "/ScriptableObjects/Loot/EchoAxe.asset");

            Assert.That(item, Is.Not.Null);
            Assert.That(item.Rarity, Is.EqualTo(LootRarity.Legendary));
            Assert.That(item.LegendaryModifier, Is.EqualTo(LegendaryModifier.EchoAxe));
        }

        [Test]
        public void CorePrefabsHaveExpectedComponents()
        {
            AssertPrefabComponent<Projectile>("ArcherProjectile.prefab");
            AssertPrefabHasComponentNamed("DamageNumber.prefab", "TextMeshPro");
            AssertPrefabComponent<LootPickup>("LootPickup_EchoAxe.prefab");
            AssertPrefabComponent<ShockwaveProjectile>("ShockwaveProjectile.prefab");
        }

        private static void AssertSkill(string fileName, AbilitySlot slot, SkillShape shape)
        {
            var skill = AssetDatabase.LoadAssetAtPath<SkillDefinition>(Root + "/ScriptableObjects/Skills/" + fileName);

            Assert.That(skill, Is.Not.Null);
            Assert.That(skill.Slot, Is.EqualTo(slot));
            Assert.That(skill.Shape, Is.EqualTo(shape));
            Assert.That(skill.Damage, Is.GreaterThan(0f));
        }

        private static void AssertEnemy(string fileName, EnemyAttackStyle attackStyle)
        {
            var enemy = AssetDatabase.LoadAssetAtPath<EnemyDefinition>(Root + "/ScriptableObjects/Enemies/" + fileName);

            Assert.That(enemy, Is.Not.Null);
            Assert.That(enemy.AttackStyle, Is.EqualTo(attackStyle));
            Assert.That(enemy.MaxHealth, Is.GreaterThan(0f));
            Assert.That(enemy.Damage, Is.GreaterThan(0f));
        }

        private static void AssertPrefabComponent<T>(string fileName) where T : Component
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(Root + "/Prefabs/" + fileName);

            Assert.That(prefab, Is.Not.Null);
            Assert.That(prefab.GetComponent<T>(), Is.Not.Null);
        }

        private static void AssertPrefabHasComponentNamed(string fileName, string componentName)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(Root + "/Prefabs/" + fileName);

            Assert.That(prefab, Is.Not.Null);
            Assert.That(prefab.GetComponent(componentName), Is.Not.Null);
        }
    }
}
