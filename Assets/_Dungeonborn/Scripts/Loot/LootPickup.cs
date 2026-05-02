using Dungeonborn.Characters;
using TMPro;
using UnityEngine;

namespace Dungeonborn.Loot
{
    public sealed class LootPickup : MonoBehaviour
    {
        [SerializeField] private LootItemDefinition item;
        [SerializeField] private TextMeshPro label;
        [SerializeField] private float rotateSpeed = 90f;
        [SerializeField] private float pickupRadius = 1.8f;
        [SerializeField] private float bobHeight = 0.25f;
        [SerializeField] private float bobSpeed = 4f;
        [SerializeField] private float spawnPulseDuration = 1.2f;

        private bool pickedUp;
        private Vector3 basePosition;
        private Vector3 baseScale;
        private Renderer pickupRenderer;
        private Color baseColor;
        private float spawnPulseRemaining;
        private global::UnityEngine.Camera mainCamera;

        public void Configure(LootItemDefinition configuredItem)
        {
            item = configuredItem;
            if (label != null && item != null)
            {
                label.text = $"{item.DisplayName}\nPick up";
                label.color = item.BeamColor;
                label.alignment = TextAlignmentOptions.Center;
                label.fontSize = 2.6f;
            }
        }

        private void Start()
        {
            Configure(item);
            EnsurePickupCollider();
            basePosition = transform.position;
            baseScale = transform.localScale;
            pickupRenderer = GetComponent<Renderer>();
            baseColor = pickupRenderer != null ? pickupRenderer.material.color : Color.white;
            spawnPulseRemaining = spawnPulseDuration;
            SpawnDropFeedback();
        }

        private void Update()
        {
            if (pickedUp)
            {
                return;
            }

            transform.position = basePosition + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
            UpdateSpawnPulse();
            FaceLabelToCamera();
            TryPickupNearestPlayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (pickedUp || item == null || !other.CompareTag("Player"))
            {
                return;
            }

            Pickup(other.gameObject);
        }

        private void TryPickupNearestPlayer()
        {
            if (pickedUp || item == null)
            {
                return;
            }

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return;
            }

            var toPlayer = player.transform.position - transform.position;
            toPlayer.y = 0f;
            var distanceSquared = toPlayer.sqrMagnitude;
            if (distanceSquared <= pickupRadius * pickupRadius)
            {
                Pickup(player);
            }
        }

        private void EnsurePickupCollider()
        {
            var sphere = GetComponent<SphereCollider>();
            if (sphere == null)
            {
                sphere = gameObject.AddComponent<SphereCollider>();
            }

            sphere.isTrigger = true;
            sphere.radius = Mathf.Max(sphere.radius, pickupRadius);
        }

        private void FaceLabelToCamera()
        {
            if (label == null)
            {
                return;
            }

            mainCamera ??= global::UnityEngine.Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            label.transform.rotation = Quaternion.LookRotation(label.transform.position - mainCamera.transform.position, Vector3.up);
        }

        private void Pickup(GameObject player)
        {
            pickedUp = true;

            if (player.TryGetComponent<PlayerLegendaryModifiers>(out var modifiers))
            {
                modifiers.Add(item.LegendaryModifier);
            }

            SpawnPickupFeedback(player.transform.position + Vector3.up * 2f);
            Destroy(gameObject);
        }

        private void SpawnPickupFeedback(Vector3 position)
        {
            var feedbackObject = new GameObject("Echo Axe Pickup Feedback");
            feedbackObject.transform.position = position;
            var text = feedbackObject.AddComponent<TextMeshPro>();
            text.text = item != null ? $"{item.DisplayName} equipped" : "Loot equipped";
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 3.8f;
            text.color = new Color(1f, 0.72f, 0.18f);
            feedbackObject.AddComponent<Dungeonborn.UI.FloatingDamageNumber>();

            var burst = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            burst.name = "Echo Axe Pickup Flash";
            burst.transform.position = position - Vector3.up * 0.8f;
            burst.transform.localScale = Vector3.one * 1.2f;
            Destroy(burst.GetComponent<Collider>());
            burst.GetComponent<Renderer>().material.color = new Color(1f, 0.72f, 0.18f);
            Destroy(burst, 0.28f);
        }

        private void UpdateSpawnPulse()
        {
            if (spawnPulseRemaining <= 0f)
            {
                transform.localScale = baseScale;
                if (pickupRenderer != null)
                {
                    pickupRenderer.material.color = baseColor;
                }

                return;
            }

            spawnPulseRemaining -= Time.deltaTime;
            var pulse = Mathf.Abs(Mathf.Sin(Time.time * 12f));
            transform.localScale = baseScale * Mathf.Lerp(1f, 1.45f, pulse);
            if (pickupRenderer != null)
            {
                pickupRenderer.material.color = Color.Lerp(baseColor, Color.white, pulse * 0.65f);
            }
        }

        private void SpawnDropFeedback()
        {
            var beam = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            beam.name = "Playtest_LootDropBeam";
            beam.transform.position = basePosition + Vector3.up * 1.2f;
            beam.transform.localScale = new Vector3(0.16f, 1.2f, 0.16f);
            Destroy(beam.GetComponent<Collider>());
            beam.GetComponent<Renderer>().material.color = new Color(1f, 0.72f, 0.18f);
            Destroy(beam, spawnPulseDuration);

            var ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ring.name = "Playtest_LootDropRing";
            ring.transform.position = basePosition + Vector3.up * 0.04f;
            ring.transform.localScale = new Vector3(1.4f, 0.04f, 1.4f);
            Destroy(ring.GetComponent<Collider>());
            ring.GetComponent<Renderer>().material.color = new Color(1f, 0.72f, 0.18f);
            Destroy(ring, spawnPulseDuration);
        }
    }
}
