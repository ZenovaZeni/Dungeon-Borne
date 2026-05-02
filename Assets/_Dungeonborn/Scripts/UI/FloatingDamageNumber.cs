using TMPro;
using UnityEngine;

namespace Dungeonborn.UI
{
    [RequireComponent(typeof(TextMeshPro))]
    public sealed class FloatingDamageNumber : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.8f;
        [SerializeField] private float riseSpeed = 1.4f;

        private UnityEngine.Camera mainCamera;
        private float remainingLifetime;

        private void Awake()
        {
            remainingLifetime = lifetime;
            mainCamera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            transform.position += Vector3.up * (riseSpeed * Time.deltaTime);

            if (mainCamera != null)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
            }

            remainingLifetime -= Time.deltaTime;
            if (remainingLifetime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
