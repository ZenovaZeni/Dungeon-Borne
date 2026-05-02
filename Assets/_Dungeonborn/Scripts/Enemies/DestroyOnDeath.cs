using Dungeonborn.Characters;
using UnityEngine;

namespace Dungeonborn.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public sealed class DestroyOnDeath : MonoBehaviour
    {
        [SerializeField] private float delay = 0.45f;
        [SerializeField] private Color deathFlashColor = new Color(1f, 0.25f, 0.1f);

        private void Awake()
        {
            GetComponent<Damageable>().Died.AddListener(HandleDeath);
        }

        private void HandleDeath()
        {
            transform.localScale *= 1.25f;
            foreach (var targetRenderer in GetComponentsInChildren<Renderer>())
            {
                targetRenderer.material.color = deathFlashColor;
            }

            Destroy(gameObject, delay);
        }
    }
}
