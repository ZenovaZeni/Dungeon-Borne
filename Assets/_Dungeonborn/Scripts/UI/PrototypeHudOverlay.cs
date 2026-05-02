using Dungeonborn.Combat;
using UnityEngine;

namespace Dungeonborn.UI
{
    public sealed class PrototypeHudOverlay : MonoBehaviour
    {
        private PlayerCombatController combat;
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

            EnsureStyles();

            const float width = 245f;
            var x = Screen.width - width - 18f;
            var y = Screen.height - 230f;

            GUI.Box(new Rect(x - 12f, y - 12f, width, 205f), string.Empty);
            GUI.Label(new Rect(x, y, width, 26f), "Prototype Controls", labelStyle);
            DrawLine(x, y + 34f, "LMB", "Basic Attack", AbilitySlot.BasicAttack);
            DrawLine(x, y + 66f, "Space", "Dash", null);
            DrawLine(x, y + 98f, "Q", "Cleave", AbilitySlot.Skill1);
            DrawLine(x, y + 130f, "E", "Stomp", AbilitySlot.Skill2);
            DrawLine(x, y + 162f, "F", "Rage", AbilitySlot.Ultimate);
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

            GUI.Label(new Rect(x + 72f, y, 160f, 24f), action + suffix, labelStyle);
        }

        private void EnsureStyles()
        {
            if (labelStyle != null)
            {
                return;
            }

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                normal = { textColor = Color.white }
            };

            keyStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
        }
    }
}
