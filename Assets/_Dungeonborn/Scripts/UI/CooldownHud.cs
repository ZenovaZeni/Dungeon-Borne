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

            SetCooldownText(basicAttackText, "ATK", AbilitySlot.BasicAttack);
            SetCooldownText(cleaveText, "CLV", AbilitySlot.Skill1);
            SetCooldownText(stompText, "STP", AbilitySlot.Skill2);
            SetCooldownText(ultimateText, "RAGE", AbilitySlot.Ultimate);
        }

        private void SetCooldownText(TextMeshProUGUI label, string name, AbilitySlot slot)
        {
            if (label == null)
            {
                return;
            }

            var remaining = combat.Cooldowns.GetRemainingSeconds(slot);
            label.text = remaining > 0f ? $"{name} {remaining:0.0}s" : $"{name} Ready";
        }
    }
}
