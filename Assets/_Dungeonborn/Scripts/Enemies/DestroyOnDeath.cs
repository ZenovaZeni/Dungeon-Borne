using Dungeonborn.Characters;
using Dungeonborn.Audio;
using UnityEngine;

namespace Dungeonborn.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public sealed class DestroyOnDeath : MonoBehaviour
    {
        [SerializeField] private float delay = 0.7f;
        [SerializeField] private Color deathFlashColor = new Color(1f, 0.25f, 0.1f);
        [SerializeField] private Color puffColor = new Color(0.55f, 0.12f, 0.08f);

        private Renderer[] renderers;
        private Vector3 startingScale;
        private float deathTimeRemaining;
        private bool dying;

        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
            startingScale = transform.localScale;
            GetComponent<Damageable>().Died.AddListener(HandleDeath);
        }

        private void Update()
        {
            if (!dying)
            {
                return;
            }

            deathTimeRemaining -= Time.deltaTime;
            var progress = 1f - Mathf.Clamp01(deathTimeRemaining / delay);
            var shrink = Mathf.Lerp(1.25f, 0.18f, progress);
            transform.localScale = startingScale * shrink;

            foreach (var targetRenderer in renderers)
            {
                if (targetRenderer != null)
                {
                    targetRenderer.material.color = Color.Lerp(deathFlashColor, Color.black, progress);
                }
            }
        }

        private void HandleDeath()
        {
            if (dying)
            {
                return;
            }

            dying = true;
            deathTimeRemaining = delay;
            PrototypeAudio.PlayEnemyDeath(transform.position);
            SpawnDeathPuffs();

            foreach (var targetRenderer in renderers)
            {
                if (targetRenderer != null)
                {
                    targetRenderer.material.color = deathFlashColor;
                }
            }

            Destroy(gameObject, delay);
        }

        private void SpawnDeathPuffs()
        {
            for (var i = 0; i < 4; i++)
            {
                var puff = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                puff.name = "Playtest_EnemyDeathPuff";
                Destroy(puff.GetComponent<Collider>());

                var angle = i * 90f * Mathf.Deg2Rad;
                var offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * 0.45f;
                puff.transform.position = transform.position + offset + Vector3.up * 0.75f;
                puff.transform.localScale = Vector3.one * 0.35f;
                puff.GetComponent<Renderer>().material.color = puffColor;
                Destroy(puff, 0.45f);
            }
        }
    }
}
