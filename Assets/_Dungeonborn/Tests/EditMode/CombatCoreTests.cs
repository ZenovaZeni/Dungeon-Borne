using Dungeonborn.Combat;
using NUnit.Framework;

namespace Dungeonborn.Tests
{
    public sealed class CombatCoreTests
    {
        [Test]
        public void CooldownBlocksUseUntilDurationExpires()
        {
            var cooldown = new CooldownTimer(2f);

            Assert.That(cooldown.TryStart(), Is.True);
            Assert.That(cooldown.TryStart(), Is.False);

            cooldown.Tick(1.5f);

            Assert.That(cooldown.TryStart(), Is.False);

            cooldown.Tick(0.5f);

            Assert.That(cooldown.TryStart(), Is.True);
        }

        [Test]
        public void HealthModelReportsDeathOnceWhenDamageDepletesHealth()
        {
            var health = new HealthModel(25f);

            var firstHit = health.ApplyDamage(10f);
            var lethalHit = health.ApplyDamage(20f);
            var overkillHit = health.ApplyDamage(20f);

            Assert.That(firstHit.WasFatal, Is.False);
            Assert.That(lethalHit.WasFatal, Is.True);
            Assert.That(overkillHit.WasFatal, Is.False);
            Assert.That(health.CurrentHealth, Is.EqualTo(0f));
            Assert.That(health.IsDead, Is.True);
        }

        [Test]
        public void HealthModelClampsInvalidMaximumToOne()
        {
            var health = new HealthModel(0f);

            Assert.That(health.MaxHealth, Is.EqualTo(1f));
            Assert.That(health.CurrentHealth, Is.EqualTo(1f));
        }

        [Test]
        public void HealthModelIgnoresZeroAndNegativeDamage()
        {
            var health = new HealthModel(10f);

            var zeroDamage = health.ApplyDamage(0f);
            var negativeDamage = health.ApplyDamage(-5f);

            Assert.That(zeroDamage.Amount, Is.EqualTo(0f));
            Assert.That(negativeDamage.Amount, Is.EqualTo(0f));
            Assert.That(health.CurrentHealth, Is.EqualTo(10f));
            Assert.That(health.IsDead, Is.False);
        }

        [Test]
        public void CooldownIgnoresNegativeTicks()
        {
            var cooldown = new CooldownTimer(2f);

            Assert.That(cooldown.TryStart(), Is.True);
            cooldown.Tick(-1f);

            Assert.That(cooldown.RemainingSeconds, Is.EqualTo(2f));
            Assert.That(cooldown.IsReady, Is.False);
        }

        [Test]
        public void ZeroDurationCooldownCanRestartImmediately()
        {
            var cooldown = new CooldownTimer(0f);

            Assert.That(cooldown.TryStart(), Is.True);
            Assert.That(cooldown.IsReady, Is.True);
            Assert.That(cooldown.TryStart(), Is.True);
        }

        [Test]
        public void EchoAxeModifierEnablesCleaveShockwave()
        {
            var modifiers = new LegendaryModifierSet(LegendaryModifier.EchoAxe);

            Assert.That(modifiers.Has(LegendaryModifier.EchoAxe), Is.True);
            Assert.That(modifiers.Has(LegendaryModifier.None), Is.False);
        }
    }
}
