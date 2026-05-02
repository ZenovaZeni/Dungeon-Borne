using Dungeonborn.Camera;
using Dungeonborn.Characters;
using Dungeonborn.Combat;
using Dungeonborn.Enemies;
using Dungeonborn.Input;
using Dungeonborn.Loot;
using Dungeonborn.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dungeonborn.Editor
{
    public static class PrototypeSceneBootstrapper
    {
        private const string Root = "Assets/_Dungeonborn";
        private const string InputAssetPath = Root + "/Input/DungeonbornControls.inputactions";
        private const string ScenePath = Root + "/Scenes/CombatSandbox_Prototype_0_1.unity";

        [MenuItem("Dungeonborn/Prototype 0.1/Generate Combat Sandbox Scene")]
        public static void GenerateCombatSandbox()
        {
            EnsureTagsAndLayers();

            var skills = CreateSkills();
            var enemies = CreateEnemies();
            var echoAxe = CreateEchoAxe();
            var materials = CreateMaterials();
            var prefabs = CreatePrefabs(materials, echoAxe);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "CombatSandbox_Prototype_0_1";

            CreateArena(materials);
            var damageNumbers = CreateDamageNumberSpawner(prefabs.DamageNumberPrefab);
            var player = CreatePlayer(skills, prefabs.ShockwavePrefab, damageNumbers);
            CreateCamera(player.transform);
            CreateLighting();
            CreateEnemies(enemies, prefabs, damageNumbers);
            CreateMobileHud();

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
            Debug.Log($"Generated Prototype 0.1 combat sandbox at {ScenePath}");
        }

        private static SkillSet CreateSkills()
        {
            return new SkillSet
            {
                BasicAttack = CreateSkill("BasicAttack.asset", "basic_attack", "Slash", AbilitySlot.BasicAttack, SkillShape.Cone, 12f, 0.35f, 2f, 1f, 80f, Color.white),
                Cleave = CreateSkill("Cleave.asset", "cleave", "Cleave", AbilitySlot.Skill1, SkillShape.Cone, 22f, 2.5f, 2.6f, 1.2f, 100f, new Color(0.4f, 0.8f, 1f)),
                Stomp = CreateSkill("Stomp.asset", "stomp", "Stomp", AbilitySlot.Skill2, SkillShape.Circle, 18f, 4f, 2.2f, 2.2f, 360f, new Color(1f, 0.75f, 0.25f)),
                Rage = CreateSkill("RagePlaceholder.asset", "rage", "Rage Placeholder", AbilitySlot.Ultimate, SkillShape.Circle, 22f, 10f, 3.6f, 3.6f, 360f, new Color(1f, 0.1f, 0.1f))
            };
        }

        private static EnemySet CreateEnemies()
        {
            return new EnemySet
            {
                Skeleton = CreateEnemy("SkeletonGrunt.asset", "skeleton_grunt", "Skeleton Grunt", EnemyAttackStyle.Melee, 35f, 3.1f, 7f, 1.5f, 1.2f, new Color(0.75f, 0.78f, 0.72f)),
                Archer = CreateEnemy("Archer.asset", "archer", "Archer", EnemyAttackStyle.Ranged, 28f, 2.6f, 6f, 7f, 1.8f, new Color(0.35f, 0.55f, 0.35f)),
                Brute = CreateEnemy("Brute.asset", "brute", "Brute", EnemyAttackStyle.HeavyMelee, 85f, 1.9f, 16f, 1.9f, 2.4f, new Color(0.5f, 0.25f, 0.22f))
            };
        }

        private static LootItemDefinition CreateEchoAxe()
        {
            var item = CreateAsset<LootItemDefinition>(Root + "/ScriptableObjects/Loot/EchoAxe.asset");
            SetSerialized(item, serialized =>
            {
                serialized.FindProperty("itemId").stringValue = "echo_axe";
                serialized.FindProperty("displayName").stringValue = "Echo Axe";
                serialized.FindProperty("rarity").enumValueIndex = (int)LootRarity.Legendary;
                serialized.FindProperty("legendaryModifier").enumValueIndex = (int)LegendaryModifier.EchoAxe;
                serialized.FindProperty("beamColor").colorValue = new Color(1f, 0.72f, 0.18f);
            });
            return item;
        }

        private static MaterialSet CreateMaterials()
        {
            return new MaterialSet
            {
                Floor = CreateMaterial("ArenaFloor.mat", new Color(0.145f, 0.15f, 0.18f)),
                Wall = CreateMaterial("ArenaWall.mat", new Color(0.075f, 0.07f, 0.085f)),
                Player = CreateMaterial("Fighter.mat", new Color(0.18f, 0.48f, 1f)),
                Skeleton = CreateMaterial("Skeleton.mat", new Color(0.9f, 0.92f, 0.84f)),
                Archer = CreateMaterial("Archer.mat", new Color(0.35f, 0.9f, 0.46f)),
                Brute = CreateMaterial("Brute.mat", new Color(0.72f, 0.26f, 0.2f)),
                Projectile = CreateMaterial("Projectile.mat", new Color(0.8f, 0.2f, 0.15f)),
                Loot = CreateMaterial("LegendaryLoot.mat", new Color(1f, 0.72f, 0.18f)),
                Shockwave = CreateMaterial("Shockwave.mat", new Color(0.35f, 0.9f, 1f))
            };
        }

        private static PrefabSet CreatePrefabs(MaterialSet materials, LootItemDefinition echoAxe)
        {
            var shockwave = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shockwave.name = "ShockwaveProjectile";
            shockwave.transform.localScale = new Vector3(1.2f, 0.2f, 0.6f);
            shockwave.GetComponent<Renderer>().sharedMaterial = materials.Shockwave;
            Object.DestroyImmediate(shockwave.GetComponent<Collider>());
            shockwave.AddComponent<ShockwaveProjectile>();
            var shockwavePrefab = SavePrefab(shockwave, Root + "/Prefabs/ShockwaveProjectile.prefab");

            var projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectile.name = "ArcherProjectile";
            projectile.transform.localScale = Vector3.one * 0.35f;
            projectile.GetComponent<Renderer>().sharedMaterial = materials.Projectile;
            Object.DestroyImmediate(projectile.GetComponent<Collider>());
            projectile.AddComponent<Projectile>();
            var projectilePrefab = SavePrefab(projectile, Root + "/Prefabs/ArcherProjectile.prefab");

            var loot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            loot.name = "LootPickup_EchoAxe";
            loot.transform.localScale = Vector3.one * 0.7f;
            loot.GetComponent<Renderer>().sharedMaterial = materials.Loot;
            var lootCollider = loot.GetComponent<Collider>();
            lootCollider.isTrigger = true;
            var lootPickup = loot.AddComponent<LootPickup>();
            SetComponentReference(lootPickup, "item", echoAxe);
            var lootLabel = new GameObject("Label").AddComponent<TextMeshPro>();
            lootLabel.transform.SetParent(loot.transform);
            lootLabel.transform.localPosition = Vector3.up * 1.2f;
            lootLabel.alignment = TextAlignmentOptions.Center;
            lootLabel.fontSize = 2f;
            SetComponentReference(lootPickup, "label", lootLabel);
            var lootPrefab = SavePrefab(loot, Root + "/Prefabs/LootPickup_EchoAxe.prefab");

            var damageNumberObject = new GameObject("DamageNumber");
            var text = damageNumberObject.AddComponent<TextMeshPro>();
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 3f;
            text.color = Color.white;
            damageNumberObject.AddComponent<FloatingDamageNumber>();
            var damageNumberPrefab = SavePrefab(damageNumberObject, Root + "/Prefabs/DamageNumber.prefab");

            return new PrefabSet
            {
                ShockwavePrefab = shockwavePrefab.GetComponent<ShockwaveProjectile>(),
                ProjectilePrefab = projectilePrefab.GetComponent<Projectile>(),
                LootPrefab = lootPrefab.GetComponent<LootPickup>(),
                DamageNumberPrefab = damageNumberPrefab.GetComponent<TextMeshPro>()
            };
        }

        private static GameObject CreatePlayer(SkillSet skills, ShockwaveProjectile shockwavePrefab, DamageNumberSpawner damageNumbers)
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Fighter_Player";
            player.tag = "Player";
            player.layer = GetLayerOrDefault("Player");
            player.transform.position = Vector3.zero;
            player.GetComponent<Renderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(Root + "/Materials/Fighter.mat");

            var capsule = player.GetComponent<CapsuleCollider>();
            Object.DestroyImmediate(capsule);
            player.AddComponent<CharacterController>();
            var input = player.AddComponent<PlayerInput>();
            input.actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputAssetPath);
            input.defaultActionMap = "Gameplay";
            input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

            player.AddComponent<PlayerInputReader>();
            player.AddComponent<PlayerMotor>();
            player.AddComponent<PlayerLegendaryModifiers>();
            player.AddComponent<Damageable>().Configure(100f);

            var combat = player.AddComponent<PlayerCombatController>();
            SetComponentReference(combat, "basicAttack", skills.BasicAttack);
            SetComponentReference(combat, "cleave", skills.Cleave);
            SetComponentReference(combat, "stomp", skills.Stomp);
            SetComponentReference(combat, "rageUltimate", skills.Rage);
            SetComponentReference(combat, "shockwavePrefab", shockwavePrefab);
            SetComponentReference(combat, "damageNumbers", damageNumbers);
            SetLayerMask(combat, "enemyLayers", "Enemy");

            return player;
        }

        private static void CreateEnemies(EnemySet enemies, PrefabSet prefabs, DamageNumberSpawner damageNumbers)
        {
            CreateEnemyInstance("Skeleton Grunt", enemies.Skeleton, new Vector3(-4f, 0f, 4f), prefabs, damageNumbers);
            CreateEnemyInstance("Archer", enemies.Archer, new Vector3(4f, 0f, 5f), prefabs, damageNumbers);
            CreateEnemyInstance("Brute", enemies.Brute, new Vector3(0f, 0f, 7f), prefabs, damageNumbers);
        }

        private static void CreateEnemyInstance(string name, EnemyDefinition definition, Vector3 position, PrefabSet prefabs, DamageNumberSpawner damageNumbers)
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = name;
            enemy.layer = GetLayerOrDefault("Enemy");
            enemy.transform.position = position;
            enemy.GetComponent<Renderer>().sharedMaterial = definition.AttackStyle == EnemyAttackStyle.Ranged
                ? AssetDatabase.LoadAssetAtPath<Material>(Root + "/Materials/Archer.mat")
                : definition.AttackStyle == EnemyAttackStyle.HeavyMelee
                    ? AssetDatabase.LoadAssetAtPath<Material>(Root + "/Materials/Brute.mat")
                    : AssetDatabase.LoadAssetAtPath<Material>(Root + "/Materials/Skeleton.mat");
            enemy.GetComponent<CapsuleCollider>().enabled = false;
            var controller = enemy.AddComponent<CharacterController>();
            controller.radius = 0.45f;
            controller.height = 2f;
            controller.center = Vector3.zero;

            var damageable = enemy.AddComponent<Damageable>();
            damageable.Configure(definition.MaxHealth);
            var brain = enemy.AddComponent<EnemyBrain>();
            SetComponentReference(brain, "definition", definition);
            SetComponentReference(brain, "projectilePrefab", prefabs.ProjectilePrefab);
            SetComponentReference(brain, "damageNumbers", damageNumbers);
            SetLayerMask(brain, "playerLayers", "Player");

            var dropper = enemy.AddComponent<LootDropper>();
            SetComponentReference(dropper, "pickupPrefab", prefabs.LootPrefab);
            if (definition.AttackStyle == EnemyAttackStyle.HeavyMelee)
            {
                SetComponentReference(dropper, "guaranteedDrop", AssetDatabase.LoadAssetAtPath<LootItemDefinition>(Root + "/ScriptableObjects/Loot/EchoAxe.asset"));
            }

            enemy.AddComponent<DestroyOnDeath>();
        }

        private static void CreateArena(MaterialSet materials)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Arena_Floor";
            floor.transform.localScale = new Vector3(18f, 0.25f, 18f);
            floor.transform.position = new Vector3(0f, -0.15f, 3f);
            floor.GetComponent<Renderer>().sharedMaterial = materials.Floor;

            CreateWall("North_Wall", new Vector3(0f, 1f, 12f), new Vector3(18f, 2f, 0.5f), materials.Wall);
            CreateWall("South_Wall", new Vector3(0f, 1f, -6f), new Vector3(18f, 2f, 0.5f), materials.Wall);
            CreateWall("West_Wall", new Vector3(-9f, 1f, 3f), new Vector3(0.5f, 2f, 18f), materials.Wall);
            CreateWall("East_Wall", new Vector3(9f, 1f, 3f), new Vector3(0.5f, 2f, 18f), materials.Wall);
        }

        private static void CreateWall(string name, Vector3 position, Vector3 scale, Material material)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = material;
        }

        private static DamageNumberSpawner CreateDamageNumberSpawner(TextMeshPro damageNumberPrefab)
        {
            var spawner = new GameObject("DamageNumberSpawner").AddComponent<DamageNumberSpawner>();
            SetComponentReference(spawner, "damageNumberPrefab", damageNumberPrefab);
            return spawner;
        }

        private static void CreateCamera(Transform target)
        {
            var cameraObject = new GameObject("Isometric Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<UnityEngine.Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 7f;
            var follow = cameraObject.AddComponent<IsometricCameraFollow>();
            SetComponentReference(follow, "target", target);
            cameraObject.transform.position = target.position + new Vector3(0f, 11f, -9f);
            cameraObject.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
        }

        private static void CreateLighting()
        {
            var lightObject = new GameObject("Key Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(0.72f, 0.78f, 1f);
            light.intensity = 0.55f;
            lightObject.transform.rotation = Quaternion.Euler(58f, -35f, 0f);
        }

        private static void CreateMobileHud()
        {
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>();

            var canvas = new GameObject("Mobile Combat HUD");
            var canvasComponent = canvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.AddComponent<GraphicRaycaster>();
            var hud = canvas.AddComponent<CooldownHud>();

            CreateOnScreenStick(canvas.transform);
            CreateOnScreenButton(canvas.transform, "Attack", "<Gamepad>/rightTrigger", new Vector2(-280f, 80f));
            CreateOnScreenButton(canvas.transform, "Dash", "<Gamepad>/buttonEast", new Vector2(-160f, 80f));
            CreateOnScreenButton(canvas.transform, "Cleave", "<Gamepad>/buttonWest", new Vector2(-280f, 200f));
            CreateOnScreenButton(canvas.transform, "Stomp", "<Gamepad>/buttonNorth", new Vector2(-160f, 200f));
            CreateOnScreenButton(canvas.transform, "Rage", "<Gamepad>/rightShoulder", new Vector2(-40f, 140f));

            SetComponentReference(hud, "basicAttackText", CreateHudLabel(canvas.transform, "AttackCooldown", new Vector2(100f, -40f), "ATK Ready"));
            SetComponentReference(hud, "cleaveText", CreateHudLabel(canvas.transform, "CleaveCooldown", new Vector2(100f, -80f), "CLV Ready"));
            SetComponentReference(hud, "stompText", CreateHudLabel(canvas.transform, "StompCooldown", new Vector2(100f, -120f), "STP Ready"));
            SetComponentReference(hud, "ultimateText", CreateHudLabel(canvas.transform, "RageCooldown", new Vector2(100f, -160f), "RAGE Ready"));
        }

        private static void CreateOnScreenStick(Transform parent)
        {
            var stick = new GameObject("Move Stick");
            stick.transform.SetParent(parent);
            var rect = stick.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 0f);
            rect.anchoredPosition = new Vector2(160f, 160f);
            rect.sizeDelta = new Vector2(160f, 160f);
            stick.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.18f);
            var onScreenStick = stick.AddComponent<OnScreenStick>();
            SetOnScreenControlPath(onScreenStick, "<Gamepad>/leftStick");
        }

        private static void CreateOnScreenButton(Transform parent, string label, string controlPath, Vector2 anchoredOffsetFromBottomRight)
        {
            var button = new GameObject(label + " Button");
            button.transform.SetParent(parent);
            var rect = button.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.anchoredPosition = anchoredOffsetFromBottomRight;
            rect.sizeDelta = new Vector2(96f, 96f);
            button.AddComponent<Image>().color = new Color(0.1f, 0.1f, 0.12f, 0.75f);
            button.AddComponent<Button>();
            var onScreenButton = button.AddComponent<OnScreenButton>();
            SetOnScreenControlPath(onScreenButton, controlPath);
            CreateHudLabel(button.transform, "Label", Vector2.zero, label);
        }

        private static TextMeshProUGUI CreateHudLabel(Transform parent, string name, Vector2 position, string text)
        {
            var labelObject = new GameObject(name);
            labelObject.transform.SetParent(parent);
            var rect = labelObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(160f, 48f);
            var label = labelObject.AddComponent<TextMeshProUGUI>();
            label.text = text;
            label.alignment = TextAlignmentOptions.Center;
            label.fontSize = 20f;
            label.color = Color.white;
            return label;
        }

        private static SkillDefinition CreateSkill(string assetName, string id, string displayName, AbilitySlot slot, SkillShape shape, float damage, float cooldown, float range, float radius, float coneAngle, Color color)
        {
            var skill = CreateAsset<SkillDefinition>(Root + "/ScriptableObjects/Skills/" + assetName);
            SetSerialized(skill, serialized =>
            {
                serialized.FindProperty("skillId").stringValue = id;
                serialized.FindProperty("displayName").stringValue = displayName;
                serialized.FindProperty("slot").enumValueIndex = (int)slot;
                serialized.FindProperty("shape").enumValueIndex = (int)shape;
                serialized.FindProperty("damage").floatValue = damage;
                serialized.FindProperty("cooldown").floatValue = cooldown;
                serialized.FindProperty("range").floatValue = range;
                serialized.FindProperty("radius").floatValue = radius;
                serialized.FindProperty("coneAngle").floatValue = coneAngle;
                serialized.FindProperty("debugColor").colorValue = color;
            });
            return skill;
        }

        private static EnemyDefinition CreateEnemy(string assetName, string id, string displayName, EnemyAttackStyle style, float health, float speed, float damage, float range, float cooldown, Color color)
        {
            var enemy = CreateAsset<EnemyDefinition>(Root + "/ScriptableObjects/Enemies/" + assetName);
            SetSerialized(enemy, serialized =>
            {
                serialized.FindProperty("enemyId").stringValue = id;
                serialized.FindProperty("displayName").stringValue = displayName;
                serialized.FindProperty("attackStyle").enumValueIndex = (int)style;
                serialized.FindProperty("maxHealth").floatValue = health;
                serialized.FindProperty("moveSpeed").floatValue = speed;
                serialized.FindProperty("damage").floatValue = damage;
                serialized.FindProperty("attackRange").floatValue = range;
                serialized.FindProperty("attackCooldown").floatValue = cooldown;
                serialized.FindProperty("bodyColor").colorValue = color;
            });
            return enemy;
        }

        private static T CreateAsset<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static Material CreateMaterial(string assetName, Color color)
        {
            var path = Root + "/Materials/" + assetName;
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
                AssetDatabase.CreateAsset(material, path);
            }

            material.color = color;
            return material;
        }

        private static GameObject SavePrefab(GameObject instance, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
            AssetDatabase.ImportAsset(path);
            return AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }

        private static void SetSerialized(Object target, System.Action<SerializedObject> configure)
        {
            var serialized = new SerializedObject(target);
            configure(serialized);
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        private static void SetComponentReference(Object component, string fieldName, Object value)
        {
            SetSerialized(component, serialized => serialized.FindProperty(fieldName).objectReferenceValue = value);
        }

        private static void SetLayerMask(Object component, string fieldName, string layerName)
        {
            var layer = GetLayerOrDefault(layerName);
            SetSerialized(component, serialized => serialized.FindProperty(fieldName).intValue = 1 << layer);
        }

        private static void SetOnScreenControlPath(Object component, string controlPath)
        {
            SetSerialized(component, serialized =>
            {
                var property = serialized.FindProperty("m_ControlPath");
                if (property != null)
                {
                    property.stringValue = controlPath;
                }
            });
        }

        private static void EnsureTagsAndLayers()
        {
            var tagManagerAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (tagManagerAssets == null || tagManagerAssets.Length == 0)
            {
                return;
            }

            var tagManager = new SerializedObject(tagManagerAssets[0]);
            EnsureTag(tagManager, "Player");
            EnsureLayer(tagManager, "Player");
            EnsureLayer(tagManager, "Enemy");
            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void EnsureTag(SerializedObject tagManager, string tag)
        {
            var tags = tagManager.FindProperty("tags");
            for (var i = 0; i < tags.arraySize; i++)
            {
                if (tags.GetArrayElementAtIndex(i).stringValue == tag)
                {
                    return;
                }
            }

            tags.InsertArrayElementAtIndex(tags.arraySize);
            tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
        }

        private static void EnsureLayer(SerializedObject tagManager, string layer)
        {
            var layers = tagManager.FindProperty("layers");
            for (var i = 0; i < layers.arraySize; i++)
            {
                if (layers.GetArrayElementAtIndex(i).stringValue == layer)
                {
                    return;
                }
            }

            for (var i = 8; i < layers.arraySize; i++)
            {
                var property = layers.GetArrayElementAtIndex(i);
                if (string.IsNullOrEmpty(property.stringValue))
                {
                    property.stringValue = layer;
                    return;
                }
            }

            Debug.LogWarning($"Could not create layer '{layer}' because all user layer slots are occupied.");
        }

        private static int GetLayerOrDefault(string layer)
        {
            var index = LayerMask.NameToLayer(layer);
            if (index >= 0)
            {
                return index;
            }

            Debug.LogWarning($"Layer '{layer}' does not exist yet; using Default layer for generated object.");
            return 0;
        }

        private sealed class SkillSet
        {
            public SkillDefinition BasicAttack;
            public SkillDefinition Cleave;
            public SkillDefinition Stomp;
            public SkillDefinition Rage;
        }

        private sealed class EnemySet
        {
            public EnemyDefinition Skeleton;
            public EnemyDefinition Archer;
            public EnemyDefinition Brute;
        }

        private sealed class MaterialSet
        {
            public Material Floor;
            public Material Wall;
            public Material Player;
            public Material Skeleton;
            public Material Archer;
            public Material Brute;
            public Material Projectile;
            public Material Loot;
            public Material Shockwave;
        }

        private sealed class PrefabSet
        {
            public ShockwaveProjectile ShockwavePrefab;
            public Projectile ProjectilePrefab;
            public LootPickup LootPrefab;
            public TextMeshPro DamageNumberPrefab;
        }
    }
}
