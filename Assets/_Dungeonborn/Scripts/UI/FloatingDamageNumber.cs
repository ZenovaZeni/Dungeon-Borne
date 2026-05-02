using TMPro;
using UnityEngine;

namespace Dungeonborn.UI
{
    [RequireComponent(typeof(TextMeshPro))]
    public sealed class FloatingDamageNumber : MonoBehaviour
    {
        [SerializeField] private float lifetime = 1f;
        [SerializeField] private float riseSpeed = 1.8f;
        [SerializeField] private float driftSpeed = 0.35f;

        private UnityEngine.Camera mainCamera;
        private float remainingLifetime;
        private Vector3 drift;

        private void Awake()
        {
            remainingLifetime = lifetime;
            mainCamera = UnityEngine.Camera.main;
            drift = new Vector3(Random.Range(-driftSpeed, driftSpeed), 0f, Random.Range(-driftSpeed, driftSpeed));
        }

        private void Update()
        {
            transform.position += (Vector3.up * riseSpeed + drift) * Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 8f);

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
