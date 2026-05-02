using Dungeonborn.Combat;
using Dungeonborn.Characters;
using UnityEngine;

namespace Dungeonborn.UI
{
    public sealed class PrototypeHudOverlay : MonoBehaviour
    {
        private PlayerCombatController combat;
        private Damageable playerDamageable;
        private GUIStyle labelStyle;
        private GUIStyle keyStyle;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateOverlay()
        {
            if (FindAnyObjectByType<PrototypeHudOverlay>() != null)
            {
                return;
            }

            var overlay = new GameObject("Prototype HUD Overlay");
            DontDestroyOnLoad(overlay);
            overlay.AddComponent<PrototypeHudOverlay>();
        }

        private void OnGUI()
        {
            if (combat == null)
            {
                combat = FindAnyObjectByType<PlayerCombatController>();
            }

            if (playerDamageable == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                playerDamageable = player != null ? player.GetComponent<Damageable>() : null;
            }

            EnsureStyles();

            const float width = 220f;
            var x = 18f;
            var y = 18f;

            GUI.Box(new Rect(x - 12f, y - 12f, width, 246f), string.Empty);
            GUI.Label(new Rect(x, y, width, 26f), "Prototype Controls", labelStyle);
            DrawLine(x, y + 32f, "LMB", "Attack", AbilitySlot.BasicAttack);
            DrawLine(x, y + 60f, "Space", "Dash", null);
            DrawLine(x, y + 88f, "Q", "Cleave", AbilitySlot.Skill1);
            DrawLine(x, y + 116f, "E", "Stomp", AbilitySlot.Skill2);
            DrawLine(x, y + 144f, "F", "Rage", AbilitySlot.Ultimate);
            DrawLine(x, y + 172f, "R", "Reset", null);
            DrawHealthLine(x, y + 204f);
        }

        private void DrawHealthLine(float x, float y)
        {
            var healthText = playerDamageable != null
                ? $"{Mathf.CeilToInt(playerDamageable.CurrentHealth)} / {Mathf.CeilToInt(playerDamageable.MaxHealth)}"
                : "-- / --";

            GUI.Label(new Rect(x, y, 62f, 24f), "HP", keyStyle);
            GUI.Label(new Rect(x + 70f, y, 145f, 24f), healthText, labelStyle);
        }

        private void DrawLine(float x, float y, string key, string action, AbilitySlot? slot)
        {
            GUI.Label(new Rect(x, y, 62f, 24f), key, keyStyle);

            var suffix = string.Empty;
            if (slot.HasValue && combat != null)
            {
                var remaining = combat.Cooldowns.GetRemainingSeconds(slot.Value);
                suffix = remaining > 0f ? $"  {remaining:0.0}s" : "  Ready";
            }

            GUI.Label(new Rect(x + 70f, y, 145f, 24f), action + suffix, labelStyle);
        }

        private void EnsureStyles()
        {
            if (labelStyle != null)
            {
                return;
            }

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                normal = { textColor = Color.white }
            };

            keyStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 13,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
        }
    }
}
