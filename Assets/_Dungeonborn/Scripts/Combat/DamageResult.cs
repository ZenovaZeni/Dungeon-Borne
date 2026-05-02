namespace Dungeonborn.Combat
{
    public readonly struct DamageResult
    {
        public DamageResult(float amount, float remainingHealth, bool wasFatal)
        {
            Amount = amount;
            RemainingHealth = remainingHealth;
            WasFatal = wasFatal;
        }

        public float Amount { get; }
        public float RemainingHealth { get; }
        public bool WasFatal { get; }
    }
}
