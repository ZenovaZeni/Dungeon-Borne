using Dungeonborn.Combat;
using UnityEngine;

namespace Dungeonborn.Characters
{
    [RequireComponent(typeof(Damageable))]
    public sealed class PlayerHitFeedback : MonoBehaviour
    {
        [SerializeField] private Color flashColor = new Color(1f, 0.08f, 0.04f);
        [SerializeField] private float ringLifetime = 0.22f;

        private Damageable damageable;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AttachToPlayer()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.GetComponent<PlayerHitFeedback>() == null)
            {
                player.AddComponent<PlayerHitFeedback>();
            }
        }

        private void Awake()
        {
            damageable = GetComponent<Damageable>();
            damageable.Damaged.AddListener(HandleDamaged);
        }

        private void OnDestroy()
        {
            if (damageable != null)
            {
                damageable.Damaged.RemoveListener(HandleDamaged);
            }
        }

        private void HandleDamaged(DamageResult result)
        {
            if (result.Amount <= 0f)
            {
                return;
            }

            SpawnHitRing();
            SpawnHitFlash();
        }

        private void SpawnHitRing()
        {
            var ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ring.name = "Playtest_PlayerHit_Ring";
            Destroy(ring.GetComponent<Collider>());
            ring.transform.position = transform.position + Vector3.up * 0.08f;
            ring.transform.localScale = new Vector3(1.7f, 0.04f, 1.7f);
            ring.GetComponent<Renderer>().material.color = flashColor;
            Destroy(ring, ringLifetime);
        }

        private void SpawnHitFlash()
        {
            var flash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flash.name = "Playtest_PlayerHit_Flash";
            Destroy(flash.GetComponent<Collider>());
            flash.transform.position = transform.position + Vector3.up * 1.05f;
            flash.transform.localScale = Vector3.one * 1.15f;
            flash.GetComponent<Renderer>().material.color = new Color(1f, 0.18f, 0.08f);
            Destroy(flash, 0.14f);
        }
    }
}
