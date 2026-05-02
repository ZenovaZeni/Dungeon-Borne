using System.IO;
using NUnit.Framework;

namespace Dungeonborn.Tests
{
    public sealed class PrototypeAssetPresenceTests
    {
        private const string Root = "Assets/_Dungeonborn";

        [Test]
        public void PrototypeSceneAndCorePrefabsExist()
        {
            Assert.That(File.Exists(Root + "/Scenes/CombatSandbox_Prototype_0_1.unity"), Is.True);
            Assert.That(File.Exists(Root + "/Prefabs/ArcherProjectile.prefab"), Is.True);
            Assert.That(File.Exists(Root + "/Prefabs/DamageNumber.prefab"), Is.True);
            Assert.That(File.Exists(Root + "/Prefabs/LootPickup_EchoAxe.prefab"), Is.True);
            Assert.That(File.Exists(Root + "/Prefabs/ShockwaveProjectile.prefab"), Is.True);
        }

        [Test]
        public void PrototypeScriptableObjectsExist()
        {
            Assert.That(File.Exists(Root + "/ScriptableObjects/Skills/BasicAttack.asset"), Is.True);
            Assert.That(File.Exists(Root + "/ScriptableObjects/Skills/Cleave.asset"), Is.True);
            Assert.That(File.Exists(Root + "/ScriptableObjects/Skills/Stomp.asset"), Is.True);
            Assert.That(File.Exists(Root + "/ScriptableObjects/Skills/RagePlaceholder.asset"), Is.True);
            Assert.That(File.Exists(Root + "/ScriptableObjects/Enemies/SkeletonGrunt.asset"), Is.True);
            Assert.That(File.Exists(Root + "/ScriptableObjects/Enemies/Archer.asset"), Is.True);
            Assert.That(File.Exists(Root + "/ScriptableObjects/Enemies/Brute.asset"), Is.True);
            Assert.That(File.Exists(Root + "/ScriptableObjects/Loot/EchoAxe.asset"), Is.True);
        }

        [Test]
        public void InputActionsContainPrototypeControls()
        {
            var inputActionsPath = Root + "/Input/DungeonbornControls.inputactions";
            Assert.That(File.Exists(inputActionsPath), Is.True);

            var inputActions = File.ReadAllText(inputActionsPath);
            Assert.That(inputActions, Does.Contain("\"name\": \"Move\""));
            Assert.That(inputActions, Does.Contain("\"name\": \"BasicAttack\""));
            Assert.That(inputActions, Does.Contain("\"name\": \"Dash\""));
            Assert.That(inputActions, Does.Contain("\"name\": \"Skill1\""));
            Assert.That(inputActions, Does.Contain("\"name\": \"Skill2\""));
            Assert.That(inputActions, Does.Contain("\"name\": \"Ultimate\""));
            Assert.That(inputActions, Does.Contain("\"name\": \"ResetSandbox\""));
            Assert.That(inputActions, Does.Contain("\"path\": \"<Keyboard>/r\""));
        }
    }
}
