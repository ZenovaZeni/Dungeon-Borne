using System;

namespace Dungeonborn.Combat
{
    [Serializable]
    public sealed class CooldownTimer
    {
        public float Duration { get; private set; }
        public float Remaining { get; private set; }
        public float RemainingSeconds => Remaining;
        public bool IsReady => Remaining <= 0f;
        public float NormalizedRemaining => Duration <= 0f ? 0f : Remaining / Duration;

        public CooldownTimer(float duration)
        {
            Duration = Math.Max(0f, duration);
            Remaining = 0f;
        }

        public bool TryStart()
        {
            if (!IsReady)
            {
                return false;
            }

            Remaining = Duration;
            return true;
        }

        public void Tick(float deltaTime)
        {
            Remaining = Math.Max(0f, Remaining - Math.Max(0f, deltaTime));
        }
    }
}
