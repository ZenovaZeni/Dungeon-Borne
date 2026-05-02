using System;

namespace Dungeonborn.Combat
{
    [Serializable]
    public sealed class HealthModel
    {
        public HealthModel(float maxHealth)
        {
            MaxHealth = Math.Max(1f, maxHealth);
            CurrentHealth = MaxHealth;
        }

        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }

        public DamageResult ApplyDamage(float amount)
        {
            if (IsDead || amount <= 0f)
            {
                return new DamageResult(0f, CurrentHealth, false);
            }

            var applied = Math.Min(CurrentHealth, amount);
            CurrentHealth = Math.Max(0f, CurrentHealth - applied);
            var wasFatal = CurrentHealth <= 0f;
            IsDead = wasFatal;

            return new DamageResult(applied, CurrentHealth, wasFatal);
        }

        public void Reset(float maxHealth)
        {
            MaxHealth = Math.Max(1f, maxHealth);
            CurrentHealth = MaxHealth;
            IsDead = false;
        }
    }
}
