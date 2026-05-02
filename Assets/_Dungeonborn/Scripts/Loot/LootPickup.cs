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

        private bool pickedUp;
        private Vector3 basePosition;
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
        }

        private void Update()
        {
            if (pickedUp)
            {
                return;
            }

            transform.position = basePosition + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
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
    }
}
