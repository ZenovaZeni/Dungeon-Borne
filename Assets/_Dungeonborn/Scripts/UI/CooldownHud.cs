using Dungeonborn.Combat;
using TMPro;
using UnityEngine;

namespace Dungeonborn.UI
{
    public sealed class CooldownHud : MonoBehaviour
    {
        [SerializeField] private PlayerCombatController combat;
        [SerializeField] private TextMeshProUGUI basicAttackText;
        [SerializeField] private TextMeshProUGUI cleaveText;
        [SerializeField] private TextMeshProUGUI stompText;
        [SerializeField] private TextMeshProUGUI ultimateText;

        private void Start()
        {
            ConfigureLabel(basicAttackText, new Vector2(-300f, 28f));
            ConfigureLabel(cleaveText, new Vector2(-300f, 148f));
            ConfigureLabel(stompText, new Vector2(-180f, 148f));
            ConfigureLabel(ultimateText, new Vector2(-60f, 88f));
        }

        private void Update()
        {
            if (combat == null)
            {
                combat = FindAnyObjectByType<PlayerCombatController>();
            }

            if (combat == null)
            {
                return;
            }

            SetCooldownText(basicAttackText, AbilitySlot.BasicAttack);
            SetCooldownText(cleaveText, AbilitySlot.Skill1);
            SetCooldownText(stompText, AbilitySlot.Skill2);
            SetCooldownText(ultimateText, AbilitySlot.Ultimate);
        }

        private void SetCooldownText(TextMeshProUGUI label, AbilitySlot slot)
        {
            if (label == null)
            {
                return;
            }

            var remaining = combat.Cooldowns.GetRemainingSeconds(slot);
            label.text = remaining > 0f ? $"{remaining:0.0}" : "READY";
        }

        private static void ConfigureLabel(TextMeshProUGUI label, Vector2 anchoredPosition)
        {
            if (label == null)
            {
                return;
            }

            var rect = label.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = new Vector2(96f, 28f);
            label.fontSize = 15f;
            label.alignment = TextAlignmentOptions.Center;
            label.color = new Color(1f, 1f, 1f, 0.92f);
        }
    }
}
