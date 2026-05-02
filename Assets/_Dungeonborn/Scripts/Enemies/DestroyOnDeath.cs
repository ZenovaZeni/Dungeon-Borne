using Dungeonborn.Characters;
using Dungeonborn.Audio;
using UnityEngine;

namespace Dungeonborn.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public sealed class DestroyOnDeath : MonoBehaviour
    {
        private const float MinimumReadableDeathDelay = 1.25f;

        [SerializeField] private float delay = MinimumReadableDeathDelay;
        [SerializeField] private Color deathFlashColor = new Color(1f, 0.25f, 0.1f);
        [SerializeField] private Color puffColor = new Color(0.55f, 0.12f, 0.08f);

        private Renderer[] renderers;
        private Vector3 startingScale;
        private Vector3 startingPosition;
        private Quaternion startingRotation;
        private Quaternion fallenRotation;
        private GameObject fallDirectionMarker;
        private float deathTimeRemaining;
        private bool dying;

        private void Awake()
        {
            delay = Mathf.Max(delay, MinimumReadableDeathDelay);
            renderers = GetComponentsInChildren<Renderer>();
            startingScale = transform.localScale;
            startingPosition = transform.position;
            startingRotation = transform.rotation;
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
            var fallProgress = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(progress / 0.32f));
            transform.rotation = Quaternion.Slerp(startingRotation, fallenRotation, fallProgress);
            transform.position = Vector3.Lerp(startingPosition, startingPosition + Vector3.down * 0.62f, fallProgress);
            transform.localScale = startingScale * Mathf.Lerp(1.12f, 1.05f, progress);

            if (fallDirectionMarker != null)
            {
                var markerProgress = Mathf.Clamp01(progress / 0.22f);
                fallDirectionMarker.transform.localScale = new Vector3(
                    Mathf.Lerp(0.12f, 0.32f, markerProgress),
                    0.05f,
                    Mathf.Lerp(0.12f, 1.25f, markerProgress));
            }

            foreach (var targetRenderer in renderers)
            {
                if (targetRenderer != null)
                {
                    targetRenderer.material.color = Color.Lerp(deathFlashColor, Color.black, Mathf.Clamp01((progress - 0.45f) / 0.55f));
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
            startingPosition = transform.position;
            startingRotation = transform.rotation;
            fallenRotation = Quaternion.AngleAxis(92f, GetFallAxis()) * startingRotation;
            DisableMotionComponents();
            PrototypeAudio.PlayEnemyDeath(transform.position);
            SpawnDeathPuffs();
            SpawnFallDirectionMarker();

            foreach (var targetRenderer in renderers)
            {
                if (targetRenderer != null)
                {
                    targetRenderer.material.color = deathFlashColor;
                }
            }

            Destroy(gameObject, delay);
        }

        private Vector3 GetFallAxis()
        {
            var side = Vector3.Cross(Vector3.up, transform.forward);
            if (side.sqrMagnitude <= 0.001f)
            {
                return Vector3.right;
            }

            return side.normalized;
        }

        private void DisableMotionComponents()
        {
            if (TryGetComponent<EnemyBrain>(out var brain))
            {
                brain.enabled = false;
            }

            if (TryGetComponent<CharacterController>(out var controller))
            {
                controller.enabled = false;
            }
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

        private void SpawnFallDirectionMarker()
        {
            fallDirectionMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fallDirectionMarker.name = "Playtest_FallDirectionMarker";
            Destroy(fallDirectionMarker.GetComponent<Collider>());
            fallDirectionMarker.transform.position = transform.position + transform.forward * 0.55f + Vector3.up * 0.08f;
            fallDirectionMarker.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
            fallDirectionMarker.transform.localScale = new Vector3(0.12f, 0.05f, 0.12f);
            fallDirectionMarker.GetComponent<Renderer>().material.color = new Color(1f, 0.16f, 0.05f);
            Destroy(fallDirectionMarker, delay);
        }
    }
}
