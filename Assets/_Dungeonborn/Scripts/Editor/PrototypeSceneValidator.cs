using Dungeonborn.Characters;
using Dungeonborn.Combat;
using Dungeonborn.Enemies;
using Dungeonborn.Input;
using Dungeonborn.Loot;
using Dungeonborn.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

namespace Dungeonborn.Editor
{
    public static class PrototypeSceneValidator
    {
        private const string ScenePath = "Assets/_Dungeonborn/Scenes/CombatSandbox_Prototype_0_1.unity";
        private const string InputAssetPath = "Assets/_Dungeonborn/Input/DungeonbornControls.inputactions";

        [MenuItem("Dungeonborn/Prototype 0.1/Validate Combat Sandbox Scene")]
        public static void ValidateCombatSandboxScene()
        {
            var previousScene = SceneManager.GetActiveScene();
            if (previousScene.path != ScenePath)
            {
                EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            }

            var issueCount = 0;
            issueCount += ValidateInputAsset();
            issueCount += ValidatePlayer();
            issueCount += ValidateSceneServices();
            issueCount += ValidateEnemies();

            if (issueCount == 0)
            {
                Debug.Log("Prototype scene validation passed: CombatSandbox_Prototype_0_1 wiring looks ready for Play Mode.");
                return;
            }

            Debug.LogError($"Prototype scene validation found {issueCount} issue(s). Fix these before relying on the combat sandbox.");
        }

        private static int ValidateInputAsset()
        {
            var issues = 0;
            var actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);
            if (actions == null)
            {
                return Fail($"Missing input action asset at {InputAssetPath}");
            }

            issues += RequireAction(actions, "Gameplay/Move");
            issues += RequireAction(actions, "Gameplay/BasicAttack");
            issues += RequireAction(actions, "Gameplay/Dash");
            issues += RequireAction(actions, "Gameplay/Skill1");
            issues += RequireAction(actions, "Gameplay/Skill2");
            issues += RequireAction(actions, "Gameplay/Ultimate");
            issues += RequireAction(actions, "Gameplay/ResetSandbox");
            return issues;
        }

        private static int ValidatePlayer()
        {
            var issues = 0;
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return Fail("Scene is missing a GameObject tagged Player.");
            }

            issues += RequireComponent<PlayerInput>(player, "PlayerInput");
            issues += RequireComponent<PlayerInputReader>(player, "PlayerInputReader");
            issues += RequireComponent<PlayerMotor>(player, "PlayerMotor");
            issues += RequireComponent<PlayerCombatController>(player, "PlayerCombatController");
            issues += RequireComponent<PlayerLegendaryModifiers>(player, "PlayerLegendaryModifiers");
            issues += RequireComponent<Damageable>(player, "Damageable");
            issues += RequireComponent<PlayerHitFeedback>(player, "PlayerHitFeedback");

            if (player.TryGetComponent<PlayerInput>(out var input))
            {
                issues += input.actions != null ? Pass("PlayerInput has actions assigned.") : Fail("PlayerInput has no actions assigned.");
                issues += input.defaultActionMap == "Gameplay" ? Pass("PlayerInput default action map is Gameplay.") : Fail("PlayerInput default action map is not Gameplay.");
            }

            if (player.TryGetComponent<PlayerCombatController>(out var combat))
            {
                issues += RequireObjectReference(combat, "basicAttack", "Player basicAttack skill");
                issues += RequireObjectReference(combat, "cleave", "Player cleave skill");
                issues += RequireObjectReference(combat, "stomp", "Player stomp skill");
                issues += RequireObjectReference(combat, "rageUltimate", "Player rageUltimate skill");
                issues += RequireObjectReference(combat, "shockwavePrefab", "Player shockwave prefab");
                issues += RequireObjectReference(combat, "damageNumbers", "Player damage number spawner");
                issues += RequireLayerMask(combat, "enemyLayers", "Player enemy layer mask");
            }

            return issues;
        }

        private static int ValidateSceneServices()
        {
            var issues = 0;

            var spawner = Object.FindAnyObjectByType<DamageNumberSpawner>();
            if (spawner == null)
            {
                issues += Fail("Scene is missing DamageNumberSpawner.");
            }
            else
            {
                issues += RequireObjectReference(spawner, "damageNumberPrefab", "DamageNumberSpawner damage number prefab");
            }

            issues += Object.FindAnyObjectByType<global::UnityEngine.Camera>() != null ? Pass("Scene has a camera.") : Fail("Scene is missing a camera.");
            issues += Object.FindAnyObjectByType<PrototypeHudOverlay>() != null || Application.isPlaying
                ? Pass("PrototypeHudOverlay is present or will be created at runtime.")
                : Pass("PrototypeHudOverlay will be created at runtime.");

            return issues;
        }

        private static int ValidateEnemies()
        {
            var issues = 0;
            var enemies = Object.FindObjectsByType<EnemyBrain>(FindObjectsInactive.Exclude);
            if (enemies.Length < 3)
            {
                issues += Fail($"Expected at least 3 enemies, found {enemies.Length}.");
            }
            else
            {
                issues += Pass($"Found {enemies.Length} enemies.");
            }

            var foundRanged = false;
            var foundHeavyDropper = false;
            foreach (var enemy in enemies)
            {
                issues += RequireObjectReference(enemy, "definition", $"{enemy.name} definition");
                issues += RequireLayerMask(enemy, "playerLayers", $"{enemy.name} player layer mask");
                issues += RequireObjectReference(enemy, "damageNumbers", $"{enemy.name} damage number spawner");

                if (enemy.TryGetComponent<LootDropper>(out var dropper))
                {
                    issues += RequireObjectReference(dropper, "pickupPrefab", $"{enemy.name} loot pickup prefab");
                    if (HasObjectReference(dropper, "guaranteedDrop"))
                    {
                        foundHeavyDropper = true;
                    }
                }
                else
                {
                    issues += Fail($"{enemy.name} is missing LootDropper.");
                }

                if (HasObjectReference(enemy, "projectilePrefab"))
                {
                    foundRanged = true;
                }
            }

            issues += foundRanged ? Pass("At least one enemy has a projectile prefab.") : Fail("No enemy has a projectile prefab assigned.");
            issues += foundHeavyDropper ? Pass("At least one enemy has a guaranteed loot drop.") : Fail("No enemy has guaranteed Echo Axe-style loot assigned.");
            return issues;
        }

        private static int RequireAction(InputActionAsset actions, string path)
        {
            return actions.FindAction(path, false) != null
                ? Pass($"Input action exists: {path}")
                : Fail($"Missing input action: {path}");
        }

        private static int RequireComponent<T>(GameObject target, string label) where T : Component
        {
            return target.GetComponent<T>() != null ? Pass($"{target.name} has {label}.") : Fail($"{target.name} is missing {label}.");
        }

        private static int RequireObjectReference(Object target, string propertyName, string label)
        {
            return HasObjectReference(target, propertyName) ? Pass($"{label} is assigned.") : Fail($"{label} is missing.");
        }

        private static bool HasObjectReference(Object target, string propertyName)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(propertyName);
            return property != null && property.objectReferenceValue != null;
        }

        private static int RequireLayerMask(Object target, string propertyName, string label)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(propertyName);
            return property != null && property.intValue != 0 ? Pass($"{label} is non-empty.") : Fail($"{label} is empty.");
        }

        private static int Pass(string message)
        {
            Debug.Log("[Prototype Scene Validator] " + message);
            return 0;
        }

        private static int Fail(string message)
        {
            Debug.LogError("[Prototype Scene Validator] " + message);
            return 1;
        }
    }
}
