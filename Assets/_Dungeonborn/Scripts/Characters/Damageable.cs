using Dungeonborn.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Dungeonborn.Characters
{
    public sealed class Damageable : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private Color hitFlashColor = Color.white;
        [SerializeField] private float hitFlashDuration = 0.12f;

        private HealthModel health;
        private Renderer[] renderers;
        private Color[] originalColors;
        private float hitFlashRemaining;

        public UnityEvent<DamageResult> Damaged = new UnityEvent<DamageResult>();
        public UnityEvent Died = new UnityEvent();

        public float MaxHealth => health?.MaxHealth ?? maxHealth;
        public float CurrentHealth => health?.CurrentHealth ?? maxHealth;
        public bool IsDead => health != null && health.IsDead;

        private void Awake()
        {
            health = new HealthModel(maxHealth);
            CacheRenderers();
        }

        private void Update()
        {
            if (hitFlashRemaining <= 0f)
            {
                return;
            }

            hitFlashRemaining -= Time.deltaTime;
            if (hitFlashRemaining <= 0f)
            {
                RestoreRendererColors();
            }
        }

        public DamageResult ApplyDamage(float amount)
        {
            var result = health.ApplyDamage(amount);

            if (result.Amount > 0f)
            {
                FlashOnHit();
                Damaged.Invoke(result);
            }

            if (result.WasFatal)
            {
                Died.Invoke();
            }

            return result;
        }

        public void Configure(float configuredMaxHealth)
        {
            maxHealth = configuredMaxHealth;
            health ??= new HealthModel(maxHealth);
            health.Reset(maxHealth);
        }

        private void CacheRenderers()
        {
            renderers = GetComponentsInChildren<Renderer>();
            originalColors = new Color[renderers.Length];
            for (var i = 0; i < renderers.Length; i++)
            {
                originalColors[i] = renderers[i].material.color;
            }
        }

        private void FlashOnHit()
        {
            if (renderers == null || renderers.Length == 0)
            {
                CacheRenderers();
            }

            hitFlashRemaining = hitFlashDuration;
            foreach (var targetRenderer in renderers)
            {
                targetRenderer.material.color = hitFlashColor;
            }
        }

        private void RestoreRendererColors()
        {
            if (renderers == null || originalColors == null)
            {
                return;
            }

            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    renderers[i].material.color = originalColors[i];
                }
            }
        }
    }
}
