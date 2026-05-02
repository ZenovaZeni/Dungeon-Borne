using Dungeonborn.Characters;
using UnityEngine;

namespace Dungeonborn.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public sealed class DestroyOnDeath : MonoBehaviour
    {
        [SerializeField] private float delay = 0.45f;

        private void Awake()
        {
            GetComponent<Damageable>().Died.AddListener(HandleDeath);
        }

        private void HandleDeath()
        {
            Destroy(gameObject, delay);
        }
    }
}
