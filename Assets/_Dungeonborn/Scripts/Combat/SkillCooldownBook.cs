using System.Collections.Generic;

namespace Dungeonborn.Combat
{
    public sealed class SkillCooldownBook
    {
        private readonly Dictionary<AbilitySlot, CooldownTimer> timers = new Dictionary<AbilitySlot, CooldownTimer>();

        public bool TryStart(SkillDefinition skill)
        {
            if (!timers.TryGetValue(skill.Slot, out var timer))
            {
                timer = new CooldownTimer(skill.Cooldown);
                timers.Add(skill.Slot, timer);
            }

            return timer.TryStart();
        }

        public void Tick(float deltaTime)
        {
            foreach (var timer in timers.Values)
            {
                timer.Tick(deltaTime);
            }
        }

        public float GetNormalizedRemaining(AbilitySlot slot)
        {
            return timers.TryGetValue(slot, out var timer) ? timer.NormalizedRemaining : 0f;
        }

        public float GetRemainingSeconds(AbilitySlot slot)
        {
            return timers.TryGetValue(slot, out var timer) ? timer.RemainingSeconds : 0f;
        }
    }
}
